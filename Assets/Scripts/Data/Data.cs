using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FruitsData
{
    public FruitsID ID;               // 과일 ID
    public string Name;               // 과일 이름
    public FruitsType Type;           // 과일 유형
    public Sprite Image;              // 과일 이미지
    public string Description;        // 과일 설명
    public int Price;                 // 과일 판매 가격
    public float Probability;         // 과일 등장 확률
}

[System.Serializable]
public class PlayerData
{
    [Header("수집된 과일 정보")]
    public Dictionary<FruitsID, CollectedFruitData> Inventory = new();

    [Header("마지막 수집 시간")]
    public DateTime LastCollectedTime;

    [Header("플레이어 지갑")]
    public int PlayerCoin = 0;

    public bool TryGetFruitData(FruitsID fruitID, out CollectedFruitData collectedFruit)
    {
        return Inventory.TryGetValue(fruitID, out collectedFruit);
    }

    public bool DecreaseFruitAmount(FruitsID fruitID, int amount)
    {
        if (Inventory.TryGetValue(fruitID, out var collectedFruit) && collectedFruit.Amount >= amount)
        {
            collectedFruit.Amount -= amount;
            if (collectedFruit.Amount <= 0)
            {
                Inventory.Remove(fruitID); // 수량이 0이면 인벤토리에서 제거
            }
            return true;
        }

        Debug.LogWarning($"인벤토리에 과일 {fruitID}의 수량을 감소시킬 수 없습니다. (현재 수량 부족)");
        return false;
    }

    public void IncreaseFruitAmount(FruitsID fruitID, int amount)
    {
        if (Inventory.TryGetValue(fruitID, out var collectedFruit))
        {
            collectedFruit.Amount += amount;
        }
        else
        {
            Inventory[fruitID] = new CollectedFruitData
            {
                ID = fruitID,
                Amount = amount
            };
        }
    }

    public void AddCoins(int amount)
    {
        PlayerCoin += amount;
    }
}

[System.Serializable]
public class CollectedFruitData
{
    public FruitsID ID;     // 과일 ID
    public int Amount;      // 보유 수량
}
