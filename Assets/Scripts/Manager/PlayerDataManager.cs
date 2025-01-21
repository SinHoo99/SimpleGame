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

    #region 데이터 초기화
    public void InitializeInventory()
    {
        if (NowPlayerData == null)
        {
            Debug.LogError("NowPlayerData가 null입니다!");
            NowPlayerData = new PlayerData(); // 기본값 생성
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

        // 현재 시간을 마지막 수집 시간으로 저장
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
            NowPlayerData = new PlayerData(); // 기본값 생성
            return false;
        }
    }
    #endregion

    #region 프리펩 데이터 저장 및 로드
    public void SavePrefabData()
    {
        List<PrefabData> prefabDataList = new List<PrefabData>();

        foreach (var obj in GM.ObjectPool.GetAllActiveObjects())
        {
            prefabDataList.Add(new PrefabData(obj.name.Replace("(Clone)", "").Trim(), obj.transform.position, obj.transform.rotation));
        }

        GM.SaveManager.SaveData(prefabDataList);
        Debug.Log("PrefabData 저장 완료");
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
                    Debug.LogWarning($"Object Pool에서 {cleanKey}을(를) 찾을 수 없습니다.");
                }
            }

            Debug.Log("PrefabData 로드 완료");
        }
        else
        {
            Debug.LogWarning("PrefabData 로드 실패 또는 데이터 없음");
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
