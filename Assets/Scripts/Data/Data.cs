using System;
using System.Collections.Generic;
using UnityEngine;


#region ���� ������
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
    public PoolObject Prefab;
}
#endregion

#region ���� ������
[System.Serializable]
public class BossData
{
    public BossID ID;               
    public int MaxHealth;
    public string AnimationState;
}
#endregion


#region ������ ������
[Serializable]
public class PrefabData
{
    public string prefabName;
    public SerializableVector3 position;
    public SerializableQuaternion rotation;

    public PrefabData(string prefabName, Vector3 position, Quaternion rotation)
    {
        this.prefabName = prefabName;
        this.position = new SerializableVector3(position);
        this.rotation = new SerializableQuaternion(rotation);
    }
}

[Serializable]
public class SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 vector)
    {
        x = vector.x;
        y = vector.y;
        z = vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }
}

[Serializable]
public class SerializableQuaternion
{
    public float x, y, z, w;

    public SerializableQuaternion(Quaternion quaternion)
    {
        x = quaternion.x;
        y = quaternion.y;
        z = quaternion.z;
        w = quaternion.w;
    }

    public Quaternion ToQuaternion()
    {
        return new Quaternion(x, y, z, w);
    }
}
#endregion

#region �÷��̾� ������
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
#endregion

#region ���� ä�� ������
[System.Serializable]
public class CollectedFruitData
{
    public FruitsID ID;     // ���� ID
    public int Amount;      // ���� ����
}
#endregion