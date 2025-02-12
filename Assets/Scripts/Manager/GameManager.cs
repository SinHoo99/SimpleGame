using System;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private OfflineScoreUpdater offlineScoreUpdater;


    [SerializeField] public UIManager uiManager { get; private set; }
    [SerializeField] public PlayerStatusUI playerStatusUI { get; private set; }
    [SerializeField] public SpawnManager spawnManager { get; private set; }
    [SerializeField] public PlayerDataManager playerDataManager { get; private set; }
    [SerializeField] public ObjectPool objectPool { get; private set; }
    [SerializeField] public DataManager dataManager { get; private set; }
    [SerializeField] public SaveManager saveManager { get; private set; }
    [SerializeField] public PoolManager poolManager { get; private set; }
    private PrefabDataManager prefabDataManager;
    private bool isQuitting = false;
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
    }
    private void Start()
    {
        if (poolManager == null)
        {
            Debug.LogError("PoolManager가 할당되지 않았습니다!");
            return;
        }
        if (objectPool == null)
        {
            Debug.LogError("ObjectPool이 할당되지 않았습니다!");
            return;
        }
        poolManager.AddObjectPool();
        prefabDataManager = new PrefabDataManager();
        playerDataManager.LoadAllData();
        playerDataManager.InitializeInventory();
        uiManager.TriggerInventoryUpdate();
    }

    #region 컴포넌트 초기화
    private void InitializeComponents()
    {
        offlineScoreUpdater = GetComponent<OfflineScoreUpdater>();
        uiManager = GetComponent<UIManager>();
        uiManager.SetFruitData(dataManager.FriutDatas);

        Debug.Log($"OfflineScoreUpdater: {(offlineScoreUpdater != null ? "초기화 완료" : "설정되지 않음")}");
        Debug.Log($"UIManager: {(uiManager != null ? "초기화 완료" : "설정되지 않음")}");
    }
    #endregion
    #region 애플리케이션 이벤트
    private void OnApplicationQuit()
    {
        isQuitting = true;
        playerDataManager.SavePlayerData();
        prefabDataManager.SavePrefabData();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause && !isQuitting)
        {
            playerDataManager.SavePlayerData();
        }
    }
    #endregion

    public FruitsData GetFruitsData(FruitsID id)
    {
        return dataManager.FriutDatas[id];
    }
}
