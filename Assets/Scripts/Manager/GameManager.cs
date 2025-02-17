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

    // �б� ���� ������Ƽ ���� (Inspector���� ���� ����������, �ܺο��� ���� �Ұ���)
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
            Debug.LogError("DataManager�� �������� �ʾҽ��ϴ�.");
            return;
        }
        InitializeComponents();
        dataManager.Initializer();

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
        poolManager.AddObjectPool();
        prefabDataManager = new PrefabDataManager();
        playerDataManager.LoadAllData();
        playerDataManager.InitializeInventory();
        uiManager.TriggerInventoryUpdate();
    }

    #region ������Ʈ �ʱ�ȭ
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

    public FruitsData GetFruitsData(FruitsID id)
    {
        return dataManager.FriutDatas[id];
    }
}
