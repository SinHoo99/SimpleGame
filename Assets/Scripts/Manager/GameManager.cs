using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private OfflineScoreUpdater offlineScoreUpdater;
    public ScoreUpdater scoreUpdater;
    public UIManager uiManager;

    private bool isQuitting = false;

    [SerializeField] private DataManager dataManager;
    [SerializeField] private SaveManager saveManager;

    public DataManager DataManager => dataManager;
    public PlayerData NowPlayerData { get; private set; }

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

        LoadAllData();

        InitializeInventory();

        if (offlineScoreUpdater != null)
        {
            offlineScoreUpdater.CollectOfflineFruits(); // �������� ���� ���� ����
        }
        else
        {
            Debug.LogWarning("OfflineScoreUpdater�� �������� �ʾҽ��ϴ�.");
        }
    }

    #region ������ �ʱ�ȭ

    /// <summary>
    /// PlayerData.Inventory�� �ʱ�ȭ�մϴ�.
    /// </summary>
    public void InitializeInventory()
    {
        if (NowPlayerData == null)
        {
            Debug.LogError("NowPlayerData�� null�Դϴ�!");
            NowPlayerData = new PlayerData(); // �⺻�� ����
        }

        if (NowPlayerData.Inventory == null)
        {
            NowPlayerData.Inventory = new Dictionary<FruitsID, CollectedFruitData>();
        }

        foreach (FruitsID id in Enum.GetValues(typeof(FruitsID)))
        {
            if (!NowPlayerData.Inventory.ContainsKey(id))
            {
                NowPlayerData.Inventory[id] = new CollectedFruitData { ID = id, Amount = 0 };
            }
        }

        if (NowPlayerData.LastCollectedTime == default)
        {
            NowPlayerData.LastCollectedTime = DateTime.Now;
        }

        Debug.Log("PlayerData.Inventory �ʱ�ȭ �Ϸ�");
    }

    #endregion

    #region ������ ����

    /// <summary>
    /// Ư�� ���� �����͸� DataManager���� �����ɴϴ�.
    /// </summary>
    public FruitsData GetFriutsData(FruitsID id)
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
        Debug.Log(offlineScoreUpdater != null
            ? "OfflineScoreUpdater �ʱ�ȭ �Ϸ�"
            : "OfflineScoreUpdater�� ã�� �� �����ϴ�!");

        scoreUpdater = GetComponent<ScoreUpdater>();
        Debug.Log(scoreUpdater != null
            ? "ScoreUpdater �ʱ�ȭ �Ϸ�"
            : "ScoreUpdater�� ã�� �� �����ϴ�!");

        uiManager = GetComponent<UIManager>();
        Debug.Log(uiManager != null
            ? "UIManager �ʱ�ȭ �Ϸ�"
            : "UIManager�� ã�� �� �����ϴ�!");

        if (uiManager != null)
        {
            uiManager.SetFruitData(dataManager.FriutDatas);
        }
    }

    #endregion

    #region ������ ���� �� �ε�

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

        // ���� �ð��� ������ ���� �ð����� ����
        NowPlayerData.LastCollectedTime = DateTime.Now;
        saveManager.SaveData(NowPlayerData);

        Debug.Log($"PlayerData ���� �Ϸ�: {NowPlayerData.LastCollectedTime}");
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

    #region  UI�� ������Ʈ�ϴ� ���� �޼���
    public void UpdateUIWithInventory()
    {
        if (uiManager == null || NowPlayerData == null || NowPlayerData.Inventory == null)
        {
            Debug.LogError("UIManager �Ǵ� Inventory�� �ʱ�ȭ���� �ʾҽ��ϴ�!");
            return;
        }

        uiManager.UpdateFruitCountsUI(
            NowPlayerData.Inventory.ToDictionary(kv => kv.Key, kv => kv.Value.Amount)
        );
    }
    #endregion
}
