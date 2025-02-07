using System.Linq;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    private float timeAccumulator = 0f; // 시간 누적 변수
    private const float collectionInterval = 1f; // 1초마다 과일 수집


    #region 과일 생성 로직
    /// <summary>
    /// 특정 과일을 추가합니다.
    /// </summary>
    public void AddFruits(FruitsID fruitID)
    {
        var inventory = GM.PlayerDataManager.NowPlayerData.Inventory;

        if (!inventory.ContainsKey(fruitID))
        {
            Debug.LogWarning($"{fruitID}가 Inventory에 존재하지 않습니다.");
            return;
        }

        // 과일 수집 처리 (Amount 증가 등)
        inventory[fruitID].Amount++;

        // 방금 수집된 과일 ID만 UI 업데이트
        GameManager.Instance.UIManager.UpdateOrCreateFruitUI(fruitID, 1);

    }

    /// <summary>
    /// 랜덤 확률 기반 과일 추가
    /// </summary>
    public void AddRandomFruit()
    {
        FruitsID? selectedFruit = GetRandomFruitByProbability();

        if (selectedFruit.HasValue)
        {
            AddFruits(selectedFruit.Value); // null이 아닐 경우만 AddFruits 호출
            GameManager.Instance.SpawnManager.SpawnFruitFromPool(selectedFruit.Value);
        }
        else
        {
            Debug.Log("아무 과일도 선택되지 않았습니다.");
        }
    }

    /// <summary>
    /// 확률 기반 랜덤 과일 선택
    /// </summary>
    private FruitsID? GetRandomFruitByProbability()
    {
        var fruits = GameManager.Instance.DataManager.FriutDatas.Values.ToList();

        // 전체 확률의 합 계산
        float totalProbability = fruits.Sum(f => f.Probability);

        // 랜덤 값 생성 (0에서 totalProbability 사이)
        float randomValue = Random.Range(0f, totalProbability + 1.0f); // +1.0f로 선택되지 않을 확률 추가

        float cumulativeProbability = 0f;

        foreach (var fruit in fruits)
        {
            cumulativeProbability += fruit.Probability;
            if (randomValue <= cumulativeProbability)
            {
                return fruit.ID; // 과일 선택
            }
        }

        // 선택되지 않을 확률
        Debug.Log("아무 과일도 선택되지 않았습니다.");
        return null; // 아무것도 선택되지 않음
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
    #endregion

    public void AddCoin()
    {
        GM.PlayerDataManager.NowPlayerData.PlayerCoin += 1;
    }

    /// <summary>
    /// 클릭 입력 처리
    /// </summary>
    public void HandleInput()
    {
        AddRandomFruit(); // 클릭 시 랜덤 과일 수집
        AddCoin();
        GameManager.Instance.UIManager.TriggerInventoryUpdate();
        GM.PlayerDataManager.NowPlayerData.PlayerCoin -= 100;
    }
}
