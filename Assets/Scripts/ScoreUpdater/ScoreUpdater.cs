using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private float timeAccumulator = 0f; // 시간 누적 변수
    private const float collectionInterval = 1f; // 1초마다 과일 수집

    private void Update()
    {
        HandleInput(); // 클릭 입력 처리
       // CollectFruitsOverTime(); // 시간에 따라 과일 수집
    }

    /// <summary>
    /// 특정 과일을 추가합니다.
    /// </summary>
    public void AddFruits(FruitsID fruitID, int count)
    {
        var inventory = GameManager.Instance.NowPlayerData.Inventory;

        if (!inventory.ContainsKey(fruitID))
        {
            Debug.LogWarning($"{fruitID}가 Inventory에 존재하지 않습니다.");
            return;
        }

        // 과일 개수 추가
        inventory[fruitID].Amount += count;
        //Debug.Log($"{fruitID} 추가됨. 현재 개수: {inventory[fruitID].Amount}");

        // UI 업데이트
        GameManager.Instance.UpdateUIWithInventory();
    }

    /// <summary>
    /// 랜덤 확률 기반 과일 추가
    /// </summary>
    public void AddRandomFruit()
    {
        // 확률 기반으로 과일 선택
        FruitsID selectedFruit = GetRandomFruitByProbability();
        AddFruits(selectedFruit, 1); // 선택된 과일을 1개 추가
    }

    /// <summary>
    /// 확률 기반 랜덤 과일 선택
    /// </summary>
    private FruitsID GetRandomFruitByProbability()
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
        return FruitsID.Apple; // 기본값
    }

    /// <summary>
    /// 클릭 입력 처리
    /// </summary>
    private void HandleInput()
    {
        if (IsInputDetected())
        {
            AddRandomFruit(); // 클릭 시 랜덤 과일 수집
        }
    }

    /// <summary>
    /// 입력 감지
    /// </summary>
    /// <returns>입력 여부</returns>
    private bool IsInputDetected()
    {
        if (Input.GetMouseButtonDown(0)) // 마우스 클릭 감지
        {
            return true;
        }

#if UNITY_EDITOR
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            return true;
        }
#endif
        return false;
    }

    /// <summary>
    /// 시간에 따라 과일 수집
    /// </summary>
    private void CollectFruitsOverTime()
    {
        timeAccumulator += Time.deltaTime;

        if (timeAccumulator >= collectionInterval)
        {
            int collectCount = Mathf.FloorToInt(timeAccumulator / collectionInterval);

            for (int i = 0; i < collectCount; i++)
            {
                AddRandomFruit();
            }

            timeAccumulator %= collectionInterval; // 초과된 시간만 남김
        }
    }
}
