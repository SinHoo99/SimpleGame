using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FruitsData
{
    public FruitsID ID;               // ���� ID
    public string Name;               // ���� �̸�
    public FruitsType Type;           // ���� ����
    public Sprite Image;              // ���� �̹���
    public string Description;        // ���� ����
    public int Price;                 // ���� �Ǹ� ����
    public float Probability;         // ���� ���� Ȯ��
    public string PrefabPath;
}

[System.Serializable]
public class PlayerData
{
    [Header("������ ���� ����")]
    public Dictionary<FruitsID, CollectedFruitData> Inventory = new();

    [Header("������ ���� �ð�")]
    public DateTime LastCollectedTime;

    [Header("�÷��̾� ����")]
    public int PlayerCoin = 0;

    public bool AddFruitAndCalculateCoins(FruitsID fruitID, int amount, Dictionary<FruitsID, FruitsData> fruitDataDictionary)
    {
        if (!fruitDataDictionary.TryGetValue(fruitID, out var fruitData))
        {
            Debug.LogWarning($"���� ������({fruitID})�� ã�� �� �����ϴ�.");
            return false;
        }

        // ���� ���� ����
        if (Inventory.TryGetValue(fruitID, out var collectedFruit))
        {
            collectedFruit.Amount -= amount;
        }
        else
        {
            Inventory[fruitID] = new CollectedFruitData { ID = fruitID, Amount = amount };
        }

        // ���� ���
        PlayerCoin += amount * fruitData.Price;

        return true;
    }
}

[System.Serializable]
public class CollectedFruitData
{
    public FruitsID ID;     // ���� ID
    public int Amount;      // ���� ����
}
