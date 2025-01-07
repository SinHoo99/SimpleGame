using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine; // Unity 엔진 클래스

public class GameManager : Singleton<GameManager>
{
    private OfflineScoreUpdater offlineScoreUpdater;
    private ScoreUpdater scoreUpdater;
    public UIManager uiManager;

    private bool isQuitting = false;

    [SerializeField] private DataManager dataManager;
    [SerializeField] private SaveManager saveManager;

    public DataManager DataManager => dataManager;



    protected override void Awake()
    {
        if (IsDuplicates()) return;

        base.Awake();

        // 모바일 테스트를 위한 프레임 설정
        Application.targetFrameRate = 60;

        // DataManager 초기화
        if (dataManager == null)
        {
            Debug.LogError("DataManager가 설정되지 않았습니다.");
            return;
        }

        dataManager.Initializer();

        // 필요한 컴포넌트 찾기
        InitializeComponents();

        // 데이터 로드
        LoadAllData();

        InitializeInventory();
    }

    #region 데이터 초기화

    /// <summary>
    /// PlayerData.Inventory를 초기화합니다.
    /// </summary>
    private void InitializeInventory()
    {
        if (NowPlayerData?.Inventory == null)
        {
            Debug.LogError("PlayerData 또는 Inventory가 null입니다!");
            return;
        }

        foreach (FriutsID id in Enum.GetValues(typeof(FriutsID)))
        {
            if (!NowPlayerData.Inventory.ContainsKey(id))
            {
                NowPlayerData.Inventory[id] = new NowFruitsData { ID = id, Amount = 0 };
            }
        }

        Debug.Log("PlayerData.Inventory 초기화 완료");
    }

    #endregion


    #region 데이터 접근

    /// <summary>
    /// 특정 과일 데이터를 DataManager에서 가져옵니다.
    /// </summary>
    public FriutsData GetFriutsData(FriutsID id)
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
        if (offlineScoreUpdater == null)
        {
            Debug.LogError("OfflineScoreUpdater를 찾을 수 없습니다.");
        }

        scoreUpdater = GetComponent<ScoreUpdater>();
        if (scoreUpdater == null)
        {
            Debug.LogError("ScoreUpdater를 찾을 수 없습니다.");
        }

        uiManager = GetComponent<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager를 찾을 수 없습니다.");
        }
        else
        {
            // InitializeFruitData 호출
            uiManager.InitializeFruitData(dataManager.FriutDatas);
            Debug.Log("UIManager에 과일 데이터 초기화 완료");
        }
        dataManager = GetComponent<DataManager>();
        saveManager = GetComponent<SaveManager>();
    }

    #endregion

    #region 데이터 저장 및 로드

    public PlayerData NowPlayerData { get; private set; }
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

        saveManager.SaveData(NowPlayerData);
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
}
