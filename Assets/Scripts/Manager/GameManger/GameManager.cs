using UnityEngine;
using UnityEngine.Audio;

public class GameManager : Singleton<GameManager>
{
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
    public ParticleSystem _particleSystem;
    [SerializeField] private PoolObject _bulletPrefabs;

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

    public SoundManager SoundManager => _soundManager;

    public AlertManager AlertManager => _alertManager;
    public ScoreUpdater ScoreUpdater => _scoreUpdater;

    private PrefabDataManager _prefabDataManager;

    public bool isQuitting = false;
    protected override void Awake()
    {
        if (IsDuplicates()) return;
        base.Awake();
        Application.targetFrameRate = 60;
        InitializeComponents();
        _dataManager.Initializer();

    }
    private void Start()
    {
        _poolManager.AddObjectPool();
        _prefabDataManager = new PrefabDataManager();
        _playerDataManager.LoadAllData();
        _playerDataManager.InitializeInventory();
        _soundManager.Initializer();
        _uiManager.InventoryManager.TriggerInventoryUpdate();
    }

    #region ������Ʈ �ʱ�ȭ
    private void InitializeComponents()
    {
        _soundManager = GetComponentInChildren<SoundManager>();
        _uiManager = GetComponentInChildren<UIManager>();
        _uiManager.InventoryManager.FruitUIManager.SetFruitData(_dataManager.FruitDatas);
        _alertManager = GetComponentInChildren<AlertManager>();
        _dataManager = GetComponentInChildren<DataManager>();
        _saveManager = GetComponentInChildren<SaveManager>();
        _spawnManager = GetComponentInChildren<SpawnManager>();
        _objectPool = GetComponentInChildren<ObjectPool>();
        _playerDataManager = GetComponentInChildren<PlayerDataManager>();
        _poolManager = GetComponentInChildren<PoolManager>();
        _bossDataManager = GetComponentInChildren<BossDataManager>();
        _particleSystem = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();
    }
    #endregion
    #region ���ø����̼� �̺�Ʈ
    private void OnApplicationQuit()
    {
        _soundManager.SettingPopup.gameObject.SetActive(false);
        isQuitting = true;
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

    #region ������ ����

    private static readonly CollectedFruitData EmptyCollectedFruitData = new CollectedFruitData { ID = FruitsID.None, Amount = 0 };
    public FruitsData GetFruitsData(FruitsID id) // ���� �Ǿ��ִ� �ʱ� ������
    {
        return _dataManager.FruitDatas[id];
    }
    public int GetFruitAmount(FruitsID id) // ���� �κ��丮�� �ִ� �� �������� ����
    {
        return _playerDataManager.NowPlayerData.Inventory.TryGetValue(id, out var collectedData) ? collectedData.Amount : 0;
    }
    public CollectedFruitData GetCollectedFruitData(FruitsID id) // ���� �κ��丮���ִ� ������
    {
        return _playerDataManager.NowPlayerData.Inventory.TryGetValue(id, out var collectedData) ? collectedData : EmptyCollectedFruitData;
    }

    public BossData GetBossData(BossID id)
    {
        if (_dataManager.BossDatas.TryGetValue(id, out BossData bossData))
        {
            return bossData;
        }
        return null;
    }
    public PoolObject GetBullet()
    {
        return _bulletPrefabs;
    }
    #endregion

    #region ����

    public AudioMixer GetAudioMixer()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError(" GameManager: Unity Editor���� Play Mode ���� ���̹Ƿ� GetAudioMixer() �������� ����!");
            return null;
        }

        if (this == null)  //  `GameManager` ��ü�� �����Ǿ����� `null` ��ȯ
        {
            Debug.LogError(" GameManager: GameManager ��ü�� ������!");
            return null;
        }

        if (_soundManager == null)
        {
            Debug.LogError(" GameManager: SoundManager�� ���� �ʱ�ȭ���� �ʾҽ��ϴ�!");
            return null;
        }

        if (_soundManager.AudioMixer == null)
        {
            Debug.LogError(" GameManager: AudioMixer�� �������� �ʾҽ��ϴ�!");
            return null;
        }
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
