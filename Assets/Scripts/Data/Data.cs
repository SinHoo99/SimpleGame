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
                Inventory.Remove(fruitID); // ������ 0�̸� �κ��丮���� ����
            }
            return true;
        }

        Debug.LogWarning($"�κ��丮�� ���� {fruitID}�� ������ ���ҽ�ų �� �����ϴ�. (���� ���� ����)");
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
    public FruitsID ID;     // ���� ID
    public int Amount;      // ���� ����
}
