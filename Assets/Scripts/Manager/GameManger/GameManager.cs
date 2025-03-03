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

    #region 컴포넌트 초기화
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
    #region 애플리케이션 이벤트
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

    #region 데이터 참조

    private static readonly CollectedFruitData EmptyCollectedFruitData = new CollectedFruitData { ID = FruitsID.None, Amount = 0 };
    public FruitsData GetFruitsData(FruitsID id) // 설정 되어있는 초기 데이터
    {
        return _dataManager.FruitDatas[id];
    }
    public int GetFruitAmount(FruitsID id) // 현재 인벤토리에 있는 각 데이터의 갯수
    {
        return _playerDataManager.NowPlayerData.Inventory.TryGetValue(id, out var collectedData) ? collectedData.Amount : 0;
    }
    public CollectedFruitData GetCollectedFruitData(FruitsID id) // 현재 인벤토리에있는 데이터
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

    #region 사운드

    public AudioMixer GetAudioMixer()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError(" GameManager: Unity Editor에서 Play Mode 종료 중이므로 GetAudioMixer() 실행하지 않음!");
            return null;
        }

        if (this == null)  //  `GameManager` 자체가 삭제되었으면 `null` 반환
        {
            Debug.LogError(" GameManager: GameManager 자체가 삭제됨!");
            return null;
        }

        if (_soundManager == null)
        {
            Debug.LogError(" GameManager: SoundManager가 아직 초기화되지 않았습니다!");
            return null;
        }

        if (_soundManager.AudioMixer == null)
        {
            Debug.LogError(" GameManager: AudioMixer가 설정되지 않았습니다!");
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
