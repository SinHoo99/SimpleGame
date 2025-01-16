using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private OfflineScoreUpdater offlineScoreUpdater;
    public ScoreUpdater scoreUpdater;
    public UIManager uiManager;

    private bool isQuitting = false;

    [SerializeField] private DataManager dataManager;
    [SerializeField] private SaveManager saveManager;

    public DataManager DataManager => dataManager;
    public PlayerData NowPlayerData { get; private set; }

    protected override void Awake()
    {
        if (IsDuplicates()) return;

        base.Awake();

        Application.targetFrameRate = 60;

        if (dataManager == null)
        {
            Debug.LogError("DataManager가 설정되지 않았습니다.");
            return;
        }

        dataManager.Initializer();

        InitializeComponents();

        LoadAllData();

        InitializeInventory();

        if (offlineScoreUpdater != null)
        {
            offlineScoreUpdater.CollectOfflineFruits(); // 오프라인 수집 로직 실행
        }
        else
        {
            Debug.LogWarning("OfflineScoreUpdater가 설정되지 않았습니다.");
        }
    }

    #region 데이터 초기화

    /// <summary>
    /// PlayerData.Inventory를 초기화합니다.
    /// </summary>
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

    #region 데이터 접근

    /// <summary>
    /// 특정 과일 데이터를 DataManager에서 가져옵니다.
    /// </summary>
    public FruitsData GetFriutsData(FruitsID id)
    {
        if (dataManager == null || !dataManager.FriutDatas.ContainsKey(id))
        {
            Debug.LogError($"DataManager에서 {id} 과일 데이터를 찾을 수 없습니다.");
            return null;
        }

        return dataManager.FriutDatas[id];
    }

    #endregion

    #region 컴포넌트 초기화

    /// <summary>
    /// 필요한 컴포넌트를 초기화합니다.
    /// </summary>
    private void InitializeComponents()
    {
        offlineScoreUpdater = GetComponent<OfflineScoreUpdater>();
        Debug.Log(offlineScoreUpdater != null
            ? "OfflineScoreUpdater 초기화 완료"
            : "OfflineScoreUpdater를 찾을 수 없습니다!");

        scoreUpdater = GetComponent<ScoreUpdater>();
        Debug.Log(scoreUpdater != null
            ? "ScoreUpdater 초기화 완료"
            : "ScoreUpdater를 찾을 수 없습니다!");

        uiManager = GetComponent<UIManager>();
        Debug.Log(uiManager != null
            ? "UIManager 초기화 완료"
            : "UIManager를 찾을 수 없습니다!");

        if (uiManager != null)
        {
            uiManager.SetFruitData(dataManager.FriutDatas);
        }
    }

    #endregion

    #region 데이터 저장 및 로드

    /// <summary>
    /// 현재 플레이어 데이터를 저장합니다.
    /// </summary>
    public void SavePlayerData()
    {
        if (NowPlayerData == null)
        {
            Debug.LogWarning("저장할 PlayerData가 없습니다.");
            return;
        }

        // 현재 시간을 마지막 수집 시간으로 저장
        NowPlayerData.LastCollectedTime = DateTime.Now;
        saveManager.SaveData(NowPlayerData);

        Debug.Log($"PlayerData 저장 완료: {NowPlayerData.LastCollectedTime}");
    }

    /// <summary>
    /// 플레이어 데이터를 로드합니다.
    /// </summary>
    public bool LoadPlayerData()
    {
        if (saveManager.TryLoadData(out PlayerData data))
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

    /// <summary>
    /// 모든 데이터를 로드합니다.
    /// </summary>
    public bool LoadAllData()
    {
        return LoadPlayerData();
    }

    #endregion

    #region 애플리케이션 이벤트

    private void OnApplicationQuit()
    {
        isQuitting = true;
        SavePlayerData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && !isQuitting)
        {
            SavePlayerData();
        }
    }

    #endregion

    #region  UI를 업데이트하는 헬퍼 메서드
    public void UpdateUIWithInventory()
    {
        if (uiManager == null || NowPlayerData == null || NowPlayerData.Inventory == null)
        {
            Debug.LogError("UIManager 또는 Inventory가 초기화되지 않았습니다!");
            return;
        }

        uiManager.UpdateFruitCountsUI(
            NowPlayerData.Inventory.ToDictionary(kv => kv.Key, kv => kv.Value.Amount)
        );
    }
    #endregion
}
