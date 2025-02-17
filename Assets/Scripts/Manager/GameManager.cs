using DG.Tweening.Core.Easing;
using System;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Progress;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private OfflineScoreUpdater offlineScoreUpdater;

    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerStatusUI playerStatusUI;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private PoolManager poolManager;

    // 읽기 전용 프로퍼티 제공 (Inspector에서 수정 가능하지만, 외부에서 변경 불가능)
    public UIManager UIManager => uiManager;
    public PlayerStatusUI PlayerStatusUI => playerStatusUI;
    public SpawnManager SpawnManager => spawnManager;
    public PlayerDataManager PlayerDataManager => playerDataManager;
    public ObjectPool ObjectPool => objectPool;
    public DataManager DataManager => dataManager;
    public SaveManager SaveManager => saveManager;
    public PoolManager PoolManager => poolManager;

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
        InitializeComponents();
        dataManager.Initializer();

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
        dataManager = GetComponent<DataManager>();
        saveManager = GetComponent<SaveManager>();
        spawnManager = GetComponent<SpawnManager>();
        objectPool = GetComponent<ObjectPool>();
        playerDataManager = GetComponent<PlayerDataManager>();
        poolManager = GetComponent<PoolManager>();
        uiManager.SetFruitData(dataManager.FriutDatas);
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
