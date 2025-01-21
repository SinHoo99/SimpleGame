using DG.Tweening.Core.Easing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;



public class PlayerDataManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    public PlayerData NowPlayerData { get; private set; }

    #region ������ �ʱ�ȭ
    public void InitializeInventory()
    {
        if (NowPlayerData == null)
        {
            Debug.LogError("NowPlayerData�� null�Դϴ�!");
            NowPlayerData = new PlayerData(); // �⺻�� ����
        }

        if (NowPlayerData.Inventory == null)
        {
            NowPlayerData.Inventory = new Dictionary<FruitsID, CollectedFruitData>();
        }

        foreach (FruitsID id in Enum.GetValues(typeof(FruitsID)))
        {
            if (!NowPlayerData.Inventory.ContainsKey(id))
            {
                NowPlayerData.Inventory[id] = new CollectedFruitData { ID = id, Amount = 0 };
            }
        }

        if (NowPlayerData.LastCollectedTime == default)
        {
            NowPlayerData.LastCollectedTime = DateTime.Now;
        }

        Debug.Log("PlayerData.Inventory �ʱ�ȭ �Ϸ�");
    }

    #endregion

    #region �÷��̾� ������ ���� �� �ε�

    public void SavePlayerData()
    {
        if (NowPlayerData == null)
        {
            Debug.LogWarning("������ PlayerData�� �����ϴ�.");
            return;
        }

        // ���� �ð��� ������ ���� �ð����� ����
        NowPlayerData.LastCollectedTime = DateTime.Now;
        GM.SaveManager.SaveData(NowPlayerData);

        Debug.Log($"PlayerData ���� �Ϸ�: {NowPlayerData.LastCollectedTime}");
    }

    public bool LoadPlayerData()
    {
        if (GM.SaveManager.TryLoadData(out PlayerData data))
        {
            NowPlayerData = data;
            return true;
        }
        else
        {
            Debug.LogWarning("PlayerData �ε忡 �����߽��ϴ�.");
            NowPlayerData = new PlayerData(); // �⺻�� ����
            return false;
        }
    }
    #endregion

    #region ������ ������ ���� �� �ε�
    public void SavePrefabData()
    {
        List<PrefabData> prefabDataList = new List<PrefabData>();

        foreach (var obj in GM.ObjectPool.GetAllActiveObjects())
        {
            prefabDataList.Add(new PrefabData(obj.name.Replace("(Clone)", "").Trim(), obj.transform.position, obj.transform.rotation));
        }

        GM.SaveManager.SaveData(prefabDataList);
        Debug.Log("PrefabData ���� �Ϸ�");
    }

    private void LoadPrefabData()
    {
        if (GM.SaveManager.TryLoadData(out List<PrefabData> prefabDataList))
        {
            foreach (var prefabData in prefabDataList)
            {
                string cleanKey = prefabData.prefabName.Trim();
                GameObject prefab = GM.ObjectPool.GetObject(cleanKey);

                if (prefab != null)
                {
                    prefab.transform.position = prefabData.position.ToVector3();
                    prefab.transform.rotation = prefabData.rotation.ToQuaternion();
                    prefab.SetActive(true);
                }
                else
                {
                    Debug.LogWarning($"Object Pool���� {cleanKey}��(��) ã�� �� �����ϴ�.");
                }
            }

            Debug.Log("PrefabData �ε� �Ϸ�");
        }
        else
        {
            Debug.LogWarning("PrefabData �ε� ���� �Ǵ� ������ ����");
        }
    }

    public bool LoadAllData()
    {
        bool playerDataLoaded = LoadPlayerData();
        LoadPrefabData();
        return playerDataLoaded;
    }

    #endregion
}
