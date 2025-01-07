using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine; // Unity ���� Ŭ����

public class GameManager : Singleton<GameManager>
{
    private OfflineScoreUpdater offlineScoreUpdater;
    private ScoreUpdater scoreUpdater;
    public UIManager uiManager;

    private bool isQuitting = false;

    [SerializeField] private DataManager dataManager;
    [SerializeField] private SaveManager saveManager;

    public DataManager DataManager => dataManager;



    protected override void Awake()
    {
        if (IsDuplicates()) return;

        base.Awake();

        // ����� �׽�Ʈ�� ���� ������ ����
        Application.targetFrameRate = 60;

        // DataManager �ʱ�ȭ
        if (dataManager == null)
        {
            Debug.LogError("DataManager�� �������� �ʾҽ��ϴ�.");
            return;
        }

        dataManager.Initializer();

        // �ʿ��� ������Ʈ ã��
        InitializeComponents();

        // ������ �ε�
        LoadAllData();

        InitializeInventory();
    }

    #region ������ �ʱ�ȭ

    /// <summary>
    /// PlayerData.Inventory�� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void InitializeInventory()
    {
        if (NowPlayerData?.Inventory == null)
        {
            Debug.LogError("PlayerData �Ǵ� Inventory�� null�Դϴ�!");
            return;
        }

        foreach (FriutsID id in Enum.GetValues(typeof(FriutsID)))
        {
            if (!NowPlayerData.Inventory.ContainsKey(id))
            {
                NowPlayerData.Inventory[id] = new NowFruitsData { ID = id, Amount = 0 };
            }
        }

        Debug.Log("PlayerData.Inventory �ʱ�ȭ �Ϸ�");
    }

    #endregion


    #region ������ ����

    /// <summary>
    /// Ư�� ���� �����͸� DataManager���� �����ɴϴ�.
    /// </summary>
    public FriutsData GetFriutsData(FriutsID id)
    {
        if (dataManager == null || !dataManager.FriutDatas.ContainsKey(id))
        {
            Debug.LogError($"DataManager���� {id} ���� �����͸� ã�� �� �����ϴ�.");
            return null;
        }

        return dataManager.FriutDatas[id];
    }

    #endregion

    #region ������Ʈ �ʱ�ȭ

    /// <summary>
    /// �ʿ��� ������Ʈ�� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void InitializeComponents()
    {
        offlineScoreUpdater = GetComponent<OfflineScoreUpdater>();
        if (offlineScoreUpdater == null)
        {
            Debug.LogError("OfflineScoreUpdater�� ã�� �� �����ϴ�.");
        }

        scoreUpdater = GetComponent<ScoreUpdater>();
        if (scoreUpdater == null)
        {
            Debug.LogError("ScoreUpdater�� ã�� �� �����ϴ�.");
        }

        uiManager = GetComponent<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager�� ã�� �� �����ϴ�.");
        }
        else
        {
            // InitializeFruitData ȣ��
            uiManager.InitializeFruitData(dataManager.FriutDatas);
            Debug.Log("UIManager�� ���� ������ �ʱ�ȭ �Ϸ�");
        }
        dataManager = GetComponent<DataManager>();
        saveManager = GetComponent<SaveManager>();
    }

    #endregion

    #region ������ ���� �� �ε�

    public PlayerData NowPlayerData { get; private set; }
    /// <summary>
    /// ���� �÷��̾� �����͸� �����մϴ�.
    /// </summary>
    public void SavePlayerData()
    {
        if (NowPlayerData == null)
        {
            Debug.LogWarning("������ PlayerData�� �����ϴ�.");
            return;
        }

        saveManager.SaveData(NowPlayerData);
    }

    /// <summary>
    /// �÷��̾� �����͸� �ε��մϴ�.
    /// </summary>
    public bool LoadPlayerData()
    {
        if (saveManager.TryLoadData(out PlayerData data))
        {
            NowPlayerData = data;
            return true;
        }
        else
        {
            Debug.LogWarning("PlayerData �ε忡 �����߽��ϴ�.");
            NowPlayerData = new PlayerData(); // �⺻�� ����
            return false;
        }
    }

    /// <summary>
    /// ��� �����͸� �ε��մϴ�.
    /// </summary>
    public bool LoadAllData()
    {
        return LoadPlayerData();
    }

    #endregion

    #region ���ø����̼� �̺�Ʈ

    private void OnApplicationQuit()
    {
        isQuitting = true;
        SavePlayerData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause && !isQuitting)
        {
            SavePlayerData();
        }
    }

    #endregion
}
