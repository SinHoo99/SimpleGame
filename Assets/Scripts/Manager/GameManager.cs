using System;
using UnityEngine;
using static UnityEditor.Progress;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private OfflineScoreUpdater offlineScoreUpdater;


    [SerializeField] public UIManager uiManager { get; private set; }
    [SerializeField] public PlayerStatusUI playerStatusUI { get; private set; }
    [SerializeField] public SpawnManager spawnManager { get; private set; }
    [SerializeField] public PlayerDataManager playerDataManager { get; private set; }
    [SerializeField] public ObjectPool objectPool { get; private set; }
    [SerializeField] public DataManager dataManager { get; private set; }
    [SerializeField] public SaveManager saveManager { get; private set; }
    [SerializeField] public PoolManager poolManager { get; private set; }
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
        uiManager.SetFruitData(dataManager.FriutDatas);

        Debug.Log($"OfflineScoreUpdater: {(offlineScoreUpdater != null ? "�ʱ�ȭ �Ϸ�" : "�������� ����")}");
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

    public FruitsData GetFruitsData(FruitsID id)
    {
        return dataManager.FriutDatas[id];
    }
}
