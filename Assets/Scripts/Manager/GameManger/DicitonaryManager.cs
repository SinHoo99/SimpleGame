using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [SerializeField] private GameObject fruitDictionaryPrefab; // 도감 UI 프리팹
    [SerializeField] private Transform dictionaryContent; // 도감이 배치될 부모 오브젝트

    private Dictionary<FruitsID, FruitDictionaryItem> _fruitDictionaryItems = new();

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
        //  이벤트 해제 (메모리 누수 방지)
        if (GM.ScoreUpdater != null)
        {
            GM.ScoreUpdater.OnFruitCollected -= UpdateDictionaryUI;
            Debug.Log("[DictionaryManager] ScoreUpdater.OnFruitCollected 이벤트 해제 완료");
        }
    }

    public void InitializeDictionary(Dictionary<FruitsID, FruitsData> fruitData)
    {
        Debug.Log("[DictionaryManager] InitializeDictionary() 실행");

        if (fruitData == null || fruitData.Count == 0)
        {
            Debug.LogWarning("[DictionaryManager] 초기화할 과일 데이터가 없습니다.");
            return;
        }

        Debug.Log($"[DictionaryManager] {fruitData.Count}개의 과일 데이터 로드 완료");

        // 기존 UI 오브젝트 정리
        foreach (var item in _fruitDictionaryItems.Values)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        _fruitDictionaryItems.Clear();

        foreach (Transform child in dictionaryContent)
        {
            Destroy(child.gameObject);
        }

        // 새로운 도감 UI 동적 생성
        foreach (var (id, data) in fruitData)
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
                continue;
            }

            fruitItem.Setup(id, data.Image);
            _fruitDictionaryItems[id] = fruitItem;
            Debug.Log($"[DictionaryManager] 도감 항목 추가: {id}");
        }

        Debug.Log("[DictionaryManager] 모든 도감 항목 생성 완료");
        UpdateAllDictionaryUI(); // 모든 UI 업데이트
    }

    //  이벤트를 통해 자동으로 실행되는 UI 업데이트 함수
    public void UpdateDictionaryUI(FruitsID fruitID)
    {
        if (!_fruitDictionaryItems.TryGetValue(fruitID, out var item))
        {
            Debug.LogWarning($"[DictionaryManager] 도감에 {fruitID} 아이템이 없습니다.");
            return;
        }

        //  강제로 최신 데이터를 가져와 UI 업데이트
        bool isCollected = GM.PlayerDataManager.IsFruitCollected(fruitID);
        Debug.Log($"[DictionaryManager] {fruitID} 수집 상태 (업데이트 직후): {isCollected}");

        item.SetCollected(isCollected);
    }


    //  모든 UI를 초기화하는 함수
    public void UpdateAllDictionaryUI()
    {
        Debug.Log("[DictionaryManager] UpdateAllDictionaryUI() 실행");

        foreach (var (id, item) in _fruitDictionaryItems)
        {
            bool isCollected = GM.PlayerDataManager.IsFruitCollected(id);
            Debug.Log($"[DictionaryManager] {id} UI 업데이트 - 수집 여부: {isCollected}");

            item.SetCollected(isCollected);
        }
    }
}
