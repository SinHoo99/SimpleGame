using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private PrefabDataManager prefabDataManager = new PrefabDataManager();

    public PlayerData NowPlayerData { get; private set; }

    #region 인벤토리 초기화
    public void InitializeInventory()
    {
        if (NowPlayerData == null)
        {
            Debug.LogError("NowPlayerData가 null입니다!");
            NowPlayerData = new PlayerData();
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

        Debug.Log("PlayerData.Inventory 초기화 완료");
    }
    #endregion

    #region 플레이어 데이터 저장 및 로드
    public void SavePlayerData()
    {
        if (NowPlayerData == null)
        {
            Debug.LogWarning("저장할 PlayerData가 없습니다.");
            return;
        }

        NowPlayerData.LastCollectedTime = DateTime.Now;
        GM.SaveManager.SaveData(NowPlayerData);
        Debug.Log($"PlayerData 저장 완료: {NowPlayerData.LastCollectedTime}");
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
            Debug.LogWarning("PlayerData 로드에 실패했습니다.");
            NowPlayerData = new PlayerData();
            return false;
        }
    }
    #endregion

    #region 전체 데이터 로드
    public bool LoadAllData()
    {
        bool playerDataLoaded = LoadPlayerData();
        prefabDataManager.LoadPrefabData(); // ObjectPool 관련 로직은 별도 관리
        return playerDataLoaded;
    }
    #endregion

    #region 데이터 삭제
    public void DestroyData()
    {
        NowPlayerData.Inventory.Clear();
        NowPlayerData.PlayerCoin = 1000;
        GM.BossDataManager.DestroyData();
        InitializeInventory();
        GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();
        GM.SpawnManager.ReturnAllFruitsToPool();
    }
    #endregion
}
