using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private OfflineScoreUpdater offlineScoreUpdater;
    [SerializeField] private ScoreUpdater scoreUpdater;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private PlayerStatusUI playerStatusUI;
    [SerializeField] private SpawnManager spawnManager;
    [SerializeField] private PlayerDataManager playerDataManager;
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private PoolManager poolManager;
    private PrefabDataManager prefabDataManager;

    #region �б�����

    public DataManager DataManager => dataManager;
    public SaveManager SaveManager => saveManager;
    public OfflineScoreUpdater OfflineScoreUpdater => offlineScoreUpdater;
    public ScoreUpdater ScoreUpdater => scoreUpdater;
    public UIManager UIManager => uiManager;
    public PlayerStatusUI PlayerStatusUI => playerStatusUI;
    public SpawnManager SpawnManager => spawnManager;
    public PlayerDataManager PlayerDataManager => playerDataManager;
    public ObjectPool ObjectPool => objectPool;
    public PoolManager PoolManager => poolManager;

    #endregion

    private bool isQuitting = false;

    protected override void Awake()
    {
        if (IsDuplicates()) return;

        base.Awake();

        Application.targetFrameRate = 60;

        if (dataManager == null)
        {
            Debug.LogError("DataManager�� �������� �ʾҽ��ϴ�.");
            return;
        }

        dataManager.Initializer();

        InitializeComponents();
    }

    private void Start()
    {
        if (poolManager == null)
        {
            Debug.LogError("PoolManager�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        if (objectPool == null)
        {
            Debug.LogError("ObjectPool�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        poolManager.InitializeObjectPool();

        prefabDataManager = new PrefabDataManager();

        playerDataManager.LoadAllData();

        playerDataManager.InitializeInventory();

        uiManager.UpdateUIWithInventory();
    }

    #region ������Ʈ �ʱ�ȭ

    private void InitializeComponents()
    {
        offlineScoreUpdater = GetComponent<OfflineScoreUpdater>();
        scoreUpdater = GetComponent<ScoreUpdater>();
        uiManager = GetComponent<UIManager>();
        uiManager.SetFruitData(dataManager.FriutDatas);

        Debug.Log($"OfflineScoreUpdater: {(offlineScoreUpdater != null ? "�ʱ�ȭ �Ϸ�" : "�������� ����")}");
        Debug.Log($"ScoreUpdater: {(scoreUpdater != null ? "�ʱ�ȭ �Ϸ�" : "�������� ����")}");
        Debug.Log($"UIManager: {(uiManager != null ? "�ʱ�ȭ �Ϸ�" : "�������� ����")}");
    }

    #endregion

    #region ���ø����̼� �̺�Ʈ

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

    #region ������ ����
    public void DestroyData()
    {
        playerDataManager.NowPlayerData.Inventory.Clear();
        Debug.Log("���� ó�� �Ǿ���");
        uiManager.UpdateUIWithInventory();
        playerDataManager.InitializeInventory();
        SpawnManager.ReturnAllFruitsToPool();
    }
    #endregion
}
