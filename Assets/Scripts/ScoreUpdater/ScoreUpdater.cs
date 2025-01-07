using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private Dictionary<FriutsID, NowFruitsData> collectionList; // 수집 목록

    private void Awake()
    {
        InitializeCollectionList();
    }
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("터치 입력 감지됨");
            AddRandomFruit(); // 랜덤 과일 수집 로직 호출
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("마우스 클릭 감지됨");
            AddRandomFruit(); // 랜덤 과일 수집 로직 호출
        }
#endif
    }
    /// <summary>
    /// 특정 과일을 추가합니다.
    /// </summary>
    public void AddFruits(FriutsID fruitID, int count)
    {
        if (!collectionList.ContainsKey(fruitID))
        {
            Debug.LogWarning($"{fruitID}가 수집 목록에 없습니다.");
            return;
        }

        // 과일 개수 증가
        collectionList[fruitID].Amount += count;
        Debug.Log($"{fruitID} 추가됨, 현재 개수: {collectionList[fruitID].Amount}");

        // UI 업데이트
        GameManager.Instance.uiManager.UpdateFruitCountsUI(
            GameManager.Instance.NowPlayerData.Inventory.ToDictionary(kv => kv.Key, kv => kv.Value.Amount)
        );
    }

    /// <summary>
    /// 랜덤 확률 기반 과일 추가
    /// </summary>
    public void AddRandomFruit()
    {
        // 확률 기반으로 과일 선택
        FriutsID selectedFruit = GetRandomFruitByProbability();
        AddFruits(selectedFruit, 1); // 선택된 과일을 1개 추가
    }

    /// <summary>
    /// 확률 기반 랜덤 과일 선택
    /// </summary>
    private FriutsID GetRandomFruitByProbability()
    {
        float totalProbability = 0;
        foreach (var fruit in GameManager.Instance.DataManager.FriutDatas.Values)
        {
            totalProbability += fruit.Probability;
        }

        float randomValue = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0;

        foreach (var fruit in GameManager.Instance.DataManager.FriutDatas.Values)
        {
            cumulativeProbability += fruit.Probability;
            if (randomValue <= cumulativeProbability)
            {
                return fruit.ID;
            }
        }

        Debug.LogWarning("확률 계산 오류: 기본값 반환");
        return FriutsID.Apple; // 기본값
    }

    /// <summary>
    /// 수집 목록 초기화
    /// </summary>
    private void InitializeCollectionList()
    {
        collectionList = GameManager.Instance.NowPlayerData.Inventory;

        foreach (FriutsID id in System.Enum.GetValues(typeof(FriutsID)))
        {
            collectionList.TryAdd(id, new NowFruitsData { ID = id, Amount = 0 });
        }

        Debug.Log("수집 목록 초기화 완료");
    }
}
