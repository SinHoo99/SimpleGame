using System;
using System.Collections.Generic;
using UnityEngine;


#region 과일 데이터
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
    public float Damage;
    public PoolObject Prefab;
}
#endregion

#region 보스 데이터
[System.Serializable]
public class BossData
{
    public BossID ID;
    public int MaxHealth;
    public string AnimationState;

    public BossData(BossID id, int maxHealth, string animationState)
    {
        ID = id;
        MaxHealth = maxHealth;
        AnimationState = animationState;
    }
}

[System.Serializable]
public class BossRuntimeData
{
    public BossID CurrentBossID;
    public float CurrentHealth;

    public BossRuntimeData(BossID id, float currentHealth)
    {
        CurrentBossID = id;
        CurrentHealth = currentHealth;
    }
}

#endregion


#region 프리펩 데이터
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

#region 플레이어 데이터
[System.Serializable]
public class PlayerData
{
    [Header("수집된 과일 정보")]
    public Dictionary<FruitsID, CollectedFruitData> Inventory = new();

    [Header("마지막 수집 시간")]
    public DateTime LastCollectedTime;

    [Header("플레이어 지갑")]
    public int PlayerCoin = 0;
}
#endregion

#region 현재 채집 데이터
[System.Serializable]
public class CollectedFruitData
{
    public FruitsID ID;     // 과일 ID
    public int Amount;      // 보유 수량
}
#endregion