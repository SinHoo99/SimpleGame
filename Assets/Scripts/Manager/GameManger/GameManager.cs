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

   public ParticleSystem _particleSystem;


    // 읽기 전용 프로퍼티 제공 (Inspector에서 수정 가능하지만, 외부에서 변경 불가능)
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

    private PrefabDataManager _prefabDataManager;

    private bool isQuitting = false;
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
        _uiManager.InventoryManager.TriggerInventoryUpdate();
    }

    #region 컴포넌트 초기화
    private void InitializeComponents()
    {
        _uiManager = GetComponentInChildren<UIManager>();
        _uiManager.InventoryManager.FruitUIManager.SetFruitData(_dataManager.FriutDatas);
        _alertManager = GetComponentInChildren<AlertManager>();
        _dataManager = GetComponentInChildren<DataManager>();
        _saveManager = GetComponentInChildren<SaveManager>();
        _spawnManager = GetComponentInChildren<SpawnManager>();
        _objectPool = GetComponentInChildren<ObjectPool>();
        _playerDataManager = GetComponentInChildren<PlayerDataManager>();
        _poolManager = GetComponentInChildren<PoolManager>();
        _bossDataManager = GetComponentInChildren<BossDataManager>();
        _soundManager = GetComponentInChildren<SoundManager>();
        _particleSystem = GameObject.FindGameObjectWithTag("Particle").GetComponent<ParticleSystem>();
    }
    #endregion
    #region 애플리케이션 이벤트
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

    #region 사운드


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
