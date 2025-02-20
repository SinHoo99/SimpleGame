using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private OfflineScoreUpdater offlineScoreUpdater;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private PlayerStatusUI _playerStatusUI;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private PlayerDataManager _playerDataManager;
    [SerializeField] private BossDataManager _bossDataManager;
    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private DataManager _dataManager;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private PoolManager _poolManager;
    public ParticleSystem _particleSystem;


    // �б� ���� ������Ƽ ���� (Inspector���� ���� ����������, �ܺο��� ���� �Ұ���)
    public UIManager UIManager => _uiManager;
    public PlayerStatusUI PlayerStatusUI => _playerStatusUI;
    public SpawnManager SpawnManager => _spawnManager;
    public PlayerDataManager PlayerDataManager => _playerDataManager;
    public ObjectPool ObjectPool => _objectPool;
    public DataManager DataManager => _dataManager;
    public SaveManager SaveManager => _saveManager;
    public PoolManager PoolManager => _poolManager;
    public ParticleSystem ParticleSystem => _particleSystem;
    public BossDataManager BossDataManager => _bossDataManager;

    private PrefabDataManager _prefabDataManager;

    private bool isQuitting = false;
    protected override void Awake()
    {
        if (IsDuplicates()) return;
        base.Awake();
        Application.targetFrameRate = 60;
        if (_dataManager == null)
        {
            Debug.LogError("DataManager�� �������� �ʾҽ��ϴ�.");
            return;
        }
        InitializeComponents();
        _dataManager.Initializer();

    }
    private void Start()
    {
        if (_poolManager == null)
        {
            Debug.LogError("PoolManager�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }
        if (_objectPool == null)
        {
            Debug.LogError("ObjectPool�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }
        _poolManager.AddObjectPool();
        _prefabDataManager = new PrefabDataManager();
        _playerDataManager.LoadAllData();
        _playerDataManager.InitializeInventory();
        _uiManager.TriggerInventoryUpdate();
    }

    #region ������Ʈ �ʱ�ȭ
    private void InitializeComponents()
    {
        offlineScoreUpdater = GetComponent<OfflineScoreUpdater>();
        _uiManager = GetComponent<UIManager>();
        _dataManager = GetComponent<DataManager>();
        _saveManager = GetComponent<SaveManager>();
        _spawnManager = GetComponent<SpawnManager>();
        _objectPool = GetComponent<ObjectPool>();
        _playerDataManager = GetComponent<PlayerDataManager>();
        _poolManager = GetComponent<PoolManager>();
        _uiManager.SetFruitData(_dataManager.FriutDatas);
        _bossDataManager = GetComponent<BossDataManager>();
        _particleSystem = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();
    }
    #endregion
    #region ���ø����̼� �̺�Ʈ
    private void OnApplicationQuit()
    {
        isQuitting = true;
        _playerDataManager.SavePlayerData();
        _prefabDataManager.SavePrefabData();
        _bossDataManager.SaveBossRuntimeData();
    }
    private void OnApplicationPause(bool pause)
    {
        if (pause && !isQuitting)
        {
            _playerDataManager.SavePlayerData();
            _bossDataManager.SaveBossRuntimeData();
        }
    }
    #endregion

    public FruitsData GetFruitsData(FruitsID id)
    {
        return _dataManager.FriutDatas[id];
    }

    public BossData GetBossData(BossID id)
    {
        if (_dataManager.BossDatas.TryGetValue(id, out BossData bossData))
        {
            return bossData;
        }
        return null;
    }

}
