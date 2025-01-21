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
    [SerializeField] private TestArea testArea;
    [SerializeField] private DataManager dataManager;
    [SerializeField] private SaveManager saveManager;

    public DataManager DataManager => dataManager;
    public SaveManager SaveManager => saveManager;
    public OfflineScoreUpdater OfflineScoreUpdater => offlineScoreUpdater;
    public ScoreUpdater ScoreUpdater => scoreUpdater;
    public UIManager UIManager => uiManager;
    public PlayerStatusUI PlayerStatusUI => playerStatusUI;
    public SpawnManager SpawnManager => spawnManager;
    public PlayerDataManager PlayerDataManager => playerDataManager;
    public ObjectPool ObjectPool => objectPool;
    public TestArea TestArea => testArea;

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

        InitializeObjectPool();

        playerDataManager.LoadAllData();

        playerDataManager.InitializeInventory();

        if (offlineScoreUpdater != null)
        {
            offlineScoreUpdater.CollectOfflineFruits(); // �������� ���� ���� ����
        }
        else
        {
            Debug.LogWarning("OfflineScoreUpdater�� �������� �ʾҽ��ϴ�.");
        }
        uiManager.UpdateUIWithInventory();
    }

    #region ������ƮǮ �ʱ�ȭ
    private void InitializeObjectPool()
    {
        if (testArea != null)
        {
            testArea.AddObjectPool();
            Debug.Log("Object Pool �ʱ�ȭ �Ϸ�");
        }
        else
        {
            Debug.LogWarning("TestArea�� �������� �ʾҽ��ϴ�. Object Pool �ʱ�ȭ ����");
        }
    }
    #endregion

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
        playerDataManager.SavePrefabData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && !isQuitting)
        {
            playerDataManager.SavePlayerData();
        }
    }
    #endregion

    #region  ������ ����
    public void DestroyData()
    {
        playerDataManager.NowPlayerData.Inventory.Clear();
        Debug.Log("���� ó�� �Ǿ���");
        uiManager.UpdateUIWithInventory();
        playerDataManager.InitializeInventory();
    }
    #endregion
}