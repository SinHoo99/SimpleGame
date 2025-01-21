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
            Debug.LogError("DataManager가 설정되지 않았습니다.");
            return;
        }

        dataManager.Initializer();

        InitializeComponents();

        InitializeObjectPool();

        playerDataManager.LoadAllData();

        playerDataManager.InitializeInventory();

        if (offlineScoreUpdater != null)
        {
            offlineScoreUpdater.CollectOfflineFruits(); // 오프라인 수집 로직 실행
        }
        else
        {
            Debug.LogWarning("OfflineScoreUpdater가 설정되지 않았습니다.");
        }
        uiManager.UpdateUIWithInventory();
    }

    #region 오브젝트풀 초기화
    private void InitializeObjectPool()
    {
        if (testArea != null)
        {
            testArea.AddObjectPool();
            Debug.Log("Object Pool 초기화 완료");
        }
        else
        {
            Debug.LogWarning("TestArea가 설정되지 않았습니다. Object Pool 초기화 실패");
        }
    }
    #endregion

    #region 컴포넌트 초기화

    private void InitializeComponents()
    {
        offlineScoreUpdater = GetComponent<OfflineScoreUpdater>();
        scoreUpdater = GetComponent<ScoreUpdater>();
        uiManager = GetComponent<UIManager>();
        uiManager.SetFruitData(dataManager.FriutDatas);
        Debug.Log($"OfflineScoreUpdater: {(offlineScoreUpdater != null ? "초기화 완료" : "설정되지 않음")}");
        Debug.Log($"ScoreUpdater: {(scoreUpdater != null ? "초기화 완료" : "설정되지 않음")}");
        Debug.Log($"UIManager: {(uiManager != null ? "초기화 완료" : "설정되지 않음")}");
    }
    #endregion

    #region 애플리케이션 이벤트

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

    #region  데이터 삭제
    public void DestroyData()
    {
        playerDataManager.NowPlayerData.Inventory.Clear();
        Debug.Log("삭제 처리 되었음");
        uiManager.UpdateUIWithInventory();
        playerDataManager.InitializeInventory();
    }
    #endregion
}