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
    public string PrefabPath;
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

    public bool AddFruitAndCalculateCoins(FruitsID fruitID, int amount, Dictionary<FruitsID, FruitsData> fruitDataDictionary)
    {
        if (!fruitDataDictionary.TryGetValue(fruitID, out var fruitData))
        {
            Debug.LogWarning($"과일 데이터({fruitID})를 찾을 수 없습니다.");
            return false;
        }

        // 과일 수량 감소
        if (Inventory.TryGetValue(fruitID, out var collectedFruit))
        {
            collectedFruit.Amount -= amount;
        }
        else
        {
            Inventory[fruitID] = new CollectedFruitData { ID = fruitID, Amount = amount };
        }

        // 코인 계산
        PlayerCoin += amount * fruitData.Price;

        return true;
    }
}

[System.Serializable]
public class CollectedFruitData
{
    public FruitsID ID;     // 과일 ID
    public int Amount;      // 보유 수량
}
