using UnityEngine;
using UnityEngine.Audio;

public class GameManager : Singleton<GameManager>
{
    [Header("Managers")]
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private PlayerStatusUI _playerStatusUI;
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private PoolManager _poolManager;
    [SerializeField] private AlertManager _alertManager;
    [SerializeField] private ObjectPool _objectPool;
    [SerializeField] private PlayerDataManager _playerDataManager;
    [SerializeField] private BossDataManager _bossDataManager;
    [SerializeField] private DataManager _dataManager;
    [SerializeField] private SaveManager _saveManager;
    [SerializeField] private ScoreUpdater _scoreUpdater;

    [Header("Game Objects")]
    public ParticleSystem _particleSystem;
    [SerializeField] private PoolObject _bulletPrefabs;

    private PrefabDataManager _prefabDataManager;
    public bool isQuitting = false;

    // Public Properties (�б� ����)
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
    public SoundManager SoundManager => _soundManager;
    public AlertManager AlertManager => _alertManager;
    public ScoreUpdater ScoreUpdater => _scoreUpdater;

    protected override void Awake()
    {
        if (IsDuplicates()) return;
        base.Awake();
        Application.targetFrameRate = 60;
        InitializeComponents();
    }

    private void Start()
    {
        InitializeGame();
    }

    #region  ���� �ʱ�ȭ ����
    private void InitializeComponents()
    {
        // 1. ������ ���� �ʱ�ȭ
        _dataManager = GetComponentInChildren<DataManager>();
        _saveManager = GetComponentInChildren<SaveManager>();
        _playerDataManager = GetComponentInChildren<PlayerDataManager>();
        _bossDataManager = GetComponentInChildren<BossDataManager>();
        _soundManager = GetComponentInChildren<SoundManager>();

        // 2. ������Ʈ Ǯ�� ���� �ʱ�ȭ
        _objectPool = GetComponentInChildren<ObjectPool>();
        _poolManager = GetComponentInChildren<PoolManager>();

        // 3. UI ���� �ʱ�ȭ
        _uiManager = GetComponentInChildren<UIManager>();

        // 4. ��Ÿ �Ŵ��� �ʱ�ȭ
        _alertManager = GetComponentInChildren<AlertManager>();
        _spawnManager = GetComponentInChildren<SpawnManager>();

        // 5. ��ƼŬ �ý��� �ʱ�ȭ
        _particleSystem = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();

        // 6. ������ �ʱ�ȭ
        _dataManager.Initialize();
        _soundManager.Initialize();
    }

    private void InitializeGame()
    {
        _poolManager.AddObjectPool();
        _playerDataManager.Initialize();
        _prefabDataManager = new PrefabDataManager();
        _uiManager.InventoryManager.TriggerInventoryUpdate();
    }
    #endregion

    #region  ���ø����̼� �̺�Ʈ
    private void OnApplicationQuit()
    {
        isQuitting = true;

        // UI ��Ȱ��ȭ (���� ����)
        if (_soundManager?.SettingPopup != null)
        {
            _soundManager.SettingPopup.gameObject.SetActive(false);
        }

        // ������ ����
        _playerDataManager.SavePlayerData();
        _prefabDataManager.SavePrefabData();
        _bossDataManager.SaveBossRuntimeData();
        _soundManager.SaveOptionData();
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

    #region  ���� ������ ����
    private static readonly CollectedFruitData EmptyCollectedFruitData = new CollectedFruitData { ID = FruitsID.None, Amount = 0 };

    public FruitsData GetFruitsData(FruitsID id)
    {
        return _dataManager.FruitDatas[id];
    }

    public int GetFruitAmount(FruitsID id)
    {
        return _playerDataManager.NowPlayerData.Inventory.TryGetValue(id, out var collectedData) ? collectedData.Amount : 0;
    }

    public CollectedFruitData GetCollectedFruitData(FruitsID id)
    {
        return _playerDataManager.NowPlayerData.Inventory.TryGetValue(id, out var collectedData) ? collectedData : EmptyCollectedFruitData;
    }

    public BossData GetBossData(BossID id)
    {
        return _dataManager.BossDatas.TryGetValue(id, out BossData bossData) ? bossData : null;
    }

    public PoolObject GetBullet()
    {
        return _bulletPrefabs;
    }
    #endregion

    #region  ���� ���� �޼���
    public AudioMixer GetAudioMixer()
    {
        return _soundManager.AudioMixer;
    }

    public void PlayBGM(BGM target)
    {
        _soundManager.PlayBGM(target);
    }

    public void PlaySFX(SFX target)
    {
        _soundManager.PlaySFX(target);
    }
    #endregion
}
