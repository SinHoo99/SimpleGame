using System;
using System.Linq;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    public event Action<FruitsID> OnFruitCollected;

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

        //  먼저 도감 등록
        GM.PlayerDataManager.CollectFruit(fruitID);

        //  데이터 저장하여 즉시 반영
        GM.PlayerDataManager.SavePlayerData();

        //  과일 수집 처리 (Amount 증가)
        inventory[fruitID].Amount++;

        //  정확한 개수를 전달하여 UI 업데이트
        GM.UIManager.InventoryManager.FruitUIManager.UpdateOrCreateFruitUI(fruitID, inventory[fruitID].Amount);

        //  이벤트를 마지막에 호출하여 UI 갱신 시 데이터가 정확하게 반영되도록 함
        OnFruitCollected?.Invoke(fruitID);

        Debug.Log($"[ScoreUpdater] {fruitID} 추가 완료 - 현재 개수: {inventory[fruitID].Amount}");
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
            GM.SpawnManager.SpawnFruitFromPool(selectedFruit.Value);
        }
    }

    /// <summary>
    /// 확률 기반 랜덤 과일 선택
    /// </summary>
    private FruitsID? GetRandomFruitByProbability()
    {
        var fruits = GM.DataManager.FruitDatas.Values.ToList();

        // 전체 확률의 합 계산
        float totalProbability = fruits.Sum(f => f.Probability);

        // 랜덤 값 생성 (0에서 totalProbability 사이)
        float randomValue = UnityEngine.Random.Range(0f, totalProbability + 1.0f); // +1.0f로 선택되지 않을 확률 추가

        float cumulativeProbability = 0f;

        foreach (var fruit in fruits)
        {
            cumulativeProbability += fruit.Probability;
            if (randomValue <= cumulativeProbability)
            {
                GM.AlertManager.ShowAlert($"{fruit.ID} 수집");
                return fruit.ID; // 과일 선택               
            }
        }

        GM.AlertManager.ShowAlert("수집에 실패했습니다.");
        return null; // 아무것도 선택되지 않음
    }
    #endregion

    #region 클릭 입력 처리
    public void HandleInput()
    {
        GM.PlaySFX(SFX.Click);

        if (GM.PlayerDataManager.NowPlayerData.PlayerCoin >= 100)
        {
            GM.PlayerStatusUI.PlayerCoin();
            AddRandomFruit(); // 클릭 시 랜덤 과일 수집
            GM.UIManager.InventoryManager.TriggerInventoryUpdate();
        }
        else
        {
            GM.AlertManager.ShowAlert("돈이 부족합니다.");
        }
    }
    #endregion
}
