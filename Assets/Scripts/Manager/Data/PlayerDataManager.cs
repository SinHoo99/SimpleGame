using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private PrefabDataManager prefabDataManager = new PrefabDataManager();

    public PlayerData NowPlayerData { get; private set; }

    #region �κ��丮 �� ���� �ʱ�ȭ

    public void Initialize()
    {
        LoadAllData();
        InitializeInventory();
    }

    public void InitializeInventory()
    {
        if (NowPlayerData == null)
        {
            Debug.LogError("NowPlayerData�� null�Դϴ�!");
            NowPlayerData = new PlayerData();
        }

        if (NowPlayerData.Inventory == null)
        {
            NowPlayerData.Inventory = new Dictionary<FruitsID, CollectedFruitData>();
        }

        //  DictionaryCollection�� null�� ���� ���� ���� (�ʱ�ȭ ����)
        if (NowPlayerData.DictionaryCollection == null)
        {
            NowPlayerData.DictionaryCollection = new Dictionary<FruitsID, bool>();

            foreach (FruitsID id in Enum.GetValues(typeof(FruitsID)))
            {
                NowPlayerData.DictionaryCollection[id] = false; // �ʱⰪ ����
            }
        }

        foreach (FruitsID id in Enum.GetValues(typeof(FruitsID)))
        {
            if (!NowPlayerData.Inventory.ContainsKey(id))
            {
                NowPlayerData.Inventory[id] = new CollectedFruitData { ID = id, Amount = 0 };
            }

            //  DictionaryCollection�� �̹� �����ϴ� ��� ���� �������� ����.
            if (!NowPlayerData.DictionaryCollection.ContainsKey(id))
            {
                NowPlayerData.DictionaryCollection[id] = false; // �⺻�� false ����
            }
        }

        if (NowPlayerData.LastCollectedTime == default)
        {
            NowPlayerData.LastCollectedTime = DateTime.Now;
        }
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

        NowPlayerData.LastCollectedTime = DateTime.Now;
        GM.SaveManager.SaveData(NowPlayerData);
       // Debug.Log($"PlayerData ���� �Ϸ�: {NowPlayerData.LastCollectedTime}");
    }

    public bool LoadPlayerData()
    {
        if (GM.SaveManager.TryLoadData(out PlayerData data))
        {
            NowPlayerData = data;
            GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();
            return true;
        }
        else
        {
            Debug.LogWarning("PlayerData �ε忡 �����߽��ϴ�.");
            NowPlayerData = new PlayerData();
            return false;
        }
    }
    #endregion

    #region ��ü ������ �ε�
    public bool LoadAllData()
    {
        bool playerDataLoaded = LoadPlayerData();
        prefabDataManager.LoadPrefabData(); // ObjectPool ���� ������ ���� ����
        return playerDataLoaded;
    }
    #endregion

    #region ������ ����
    public void DestroyData()
    {
        NowPlayerData.Inventory.Clear();
        NowPlayerData.DictionaryCollection.Clear();
        NowPlayerData.PlayerCoin = 1000;
        GM.BossDataManager.DestroyData();
        InitializeInventory();
        GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();
        GM.SpawnManager.ReturnAllFruitsToPool();
    }
    #endregion

    #region ���� ���� ���
    // ���� ���� �� �������� �ݿ�
    public void CollectFruit(FruitsID fruitID)
    {
        if (!NowPlayerData.DictionaryCollection.ContainsKey(fruitID) || !NowPlayerData.DictionaryCollection[fruitID])
        {
            NowPlayerData.DictionaryCollection[fruitID] = true;
            Debug.Log($"���� ���: {fruitID}");
            Debug.Log($"{NowPlayerData.DictionaryCollection[fruitID]}");
            SavePlayerData();
        }
        else
        {
            Debug.Log($"[PlayerDataManager] �̹� ������ ��ϵ�: {fruitID}");
        }
    }

    #endregion
}

