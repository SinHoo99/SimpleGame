using UnityEngine;

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
    private PrefabDataManager prefabDataManager;
    #region 읽기전용
    public DataManager DataManager => dataManager;
    public SaveManager SaveManager => saveManager;
    public UIManager UIManager => uiManager;
    public PlayerStatusUI PlayerStatusUI => playerStatusUI;
    public SpawnManager SpawnManager => spawnManager;
    public PlayerDataManager PlayerDataManager => playerDataManager;
    public ObjectPool ObjectPool => objectPool;
    #endregion
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
        poolManager.InitializeObjectPool();
        prefabDataManager = new PrefabDataManager();
        playerDataManager.LoadAllData();
        playerDataManager.InitializeInventory();
        uiManager.UpdateUIWithInventory();
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
}
