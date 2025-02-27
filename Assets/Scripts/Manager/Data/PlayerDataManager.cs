using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private PrefabDataManager prefabDataManager = new PrefabDataManager();

    public PlayerData NowPlayerData { get; private set; }

    #region 인벤토리 및 도감 초기화
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

        if (NowPlayerData.DictionaryCollection == null)
        {
            NowPlayerData.DictionaryCollection = new Dictionary<FruitsID, bool>();
        }

        foreach (FruitsID id in Enum.GetValues(typeof(FruitsID)))
        {
            if (!NowPlayerData.Inventory.ContainsKey(id))
            {
                NowPlayerData.Inventory[id] = new CollectedFruitData { ID = id, Amount = 0 };
            }

            if (!NowPlayerData.DictionaryCollection.ContainsKey(id))
            {
                NowPlayerData.DictionaryCollection[id] = false; // 기본적으로 미수집 상태
            }
        }

        if (NowPlayerData.LastCollectedTime == default)
        {
            NowPlayerData.LastCollectedTime = DateTime.Now;
        }

        Debug.Log("PlayerData.Inventory 및 DictionaryCollection 초기화 완료");
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
        NowPlayerData.DictionaryCollection.Clear();
        NowPlayerData.PlayerCoin = 1000;
        GM.BossDataManager.DestroyData();
        InitializeInventory();
        GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();
        GM.SpawnManager.ReturnAllFruitsToPool();
    }
    #endregion

    #region 도감 관련 기능
    // 과일 수집 시 도감에도 반영
    public void CollectFruit(FruitsID fruitID)
    {
        if (!NowPlayerData.DictionaryCollection.ContainsKey(fruitID) || !NowPlayerData.DictionaryCollection[fruitID])
        {
            NowPlayerData.DictionaryCollection[fruitID] = true;
            Debug.Log($"도감 등록: {fruitID}");
            SavePlayerData();
        }
        else
        {
            Debug.Log($"[PlayerDataManager] 이미 도감에 등록됨: {fruitID}");
        }
    }

    // 도감에 등록된 과일인지 확인
    public bool IsFruitCollected(FruitsID fruitID)
    {
        return NowPlayerData.DictionaryCollection.TryGetValue(fruitID, out bool collected) && collected;
    }
    #endregion
}

