using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager : MonoBehaviour
{
    private GameManager _gm;
    private GameManager GM => _gm ??= GameManager.Instance; //  안전한 싱글톤 참조

    [SerializeField] private GameObject fruitDictionaryPrefab; // 도감 UI 프리팹
    [SerializeField] private Transform dictionaryContent; // 도감 UI 부모 객체

    private readonly Dictionary<FruitsID, FruitDictionaryItem> _fruitDictionaryItems = new();

    private void Start()
    {
        Debug.Log("[DictionaryManager] Start() 실행");

        if (GM == null || GM.DataManager == null || GM.ScoreUpdater == null)
        {
            Debug.LogError("[DictionaryManager] GameManager, DataManager 또는 ScoreUpdater가 존재하지 않습니다.");
            return;
        }

        InitializeDictionary(GM.DataManager.FruitDatas);

        //  ScoreUpdater의 과일 추가 이벤트 구독 (UI 자동 업데이트)
        GM.ScoreUpdater.OnFruitCollected += UpdateDictionaryUI;
        Debug.Log("[DictionaryManager] ScoreUpdater.OnFruitCollected 이벤트 구독 완료");
    }

    private void OnDestroy()
    {
        if (GM != null && GM.ScoreUpdater != null)
        {
            GM.ScoreUpdater.OnFruitCollected -= UpdateDictionaryUI;
            Debug.Log("[DictionaryManager] ScoreUpdater.OnFruitCollected 이벤트 해제 완료");
        }
    }

    /// 도감 UI를 초기화
    public void InitializeDictionary(Dictionary<FruitsID, FruitsData> fruitData)
    {
        if (fruitData == null || fruitData.Count == 0) return;

        ClearExistingUI();

        foreach (var (id, data) in fruitData)
        {
            CreateFruitDictionaryItem(id, data);
        }

        UpdateAllDictionaryUI();
    }

    /// 기존 UI 요소들을 제거
    private void ClearExistingUI()
    {
        foreach (var item in _fruitDictionaryItems.Values)
        {
            if (item != null) Destroy(item.gameObject);
        }
        _fruitDictionaryItems.Clear();

        foreach (Transform child in dictionaryContent)
        {
            Destroy(child.gameObject);
        }
    }

    /// 새로운 도감 UI 항목을 생성
    private void CreateFruitDictionaryItem(FruitsID id, FruitsData data)
    {
        if (fruitDictionaryPrefab == null)
        {
            Debug.LogError("[DictionaryManager] fruitDictionaryPrefab이 설정되지 않았습니다.");
            return;
        }

        var itemObj = Instantiate(fruitDictionaryPrefab, dictionaryContent);
        if (!itemObj.TryGetComponent<FruitDictionaryItem>(out var fruitItem))
        {
            Debug.LogError($"[DictionaryManager] {id}에 대한 FruitDictionaryItem 스크립트가 없습니다!");
            Destroy(itemObj);
            return;
        }

        fruitItem.Setup(id, data.Image);
        _fruitDictionaryItems[id] = fruitItem;
    }

    /// 특정 과일의 UI를 업데이트
    public void UpdateDictionaryUI(FruitsID fruitID)
    {
        if (!_fruitDictionaryItems.ContainsKey(fruitID)) return;

        var item = _fruitDictionaryItems[fruitID];

        //  인벤토리에 과일 개수가 0이어도, 도감에 등록된 적이 있다면 `true`
        bool isCollected = GM.PlayerDataManager.NowPlayerData.DictionaryCollection.TryGetValue(fruitID, out bool collected)
                            ? collected : false;

        Debug.Log($"[UpdateDictionaryUI] {fruitID} - 도감 데이터: {isCollected}");
        item.UpdateFruitUI(isCollected);
    }


    /// 전체 도감 UI를 업데이트
    public void UpdateAllDictionaryUI()
    {
        foreach (var (id, item) in _fruitDictionaryItems)
        {
            bool isCollected = GM.PlayerDataManager.NowPlayerData.DictionaryCollection.TryGetValue(id, out bool collected)
                                ? collected : false;

            Debug.Log($"[UpdateAllDictionaryUI] {id} - 도감 데이터: {isCollected}");
            item.UpdateFruitUI(isCollected);
        }
    }
}
