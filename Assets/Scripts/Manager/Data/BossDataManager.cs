using System;
using UnityEngine;

public class BossDataManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    public BossData StaticBossData { get; private set; }
    public BossRuntimeData BossRuntimeData { get; private set; }

    private void Awake()
    {
        LoadAllData();
    }

    #region  ���� ������ ���� �� �ε�

    //  ������ ���� ������ �ε� (CSV���� ��������)
    public bool LoadBossData(BossID bossID)
    {
        BossData bossData = GM.GetBossData(bossID);
        if (bossData != null)
        {
            StaticBossData = bossData;
          //  Debug.Log($"[BossDataManager] ���� ������ �ε� �Ϸ�: ID={bossID}");
            return true;
        }
        else
        {
            Debug.LogWarning($"[BossDataManager] ���� �����͸� ã�� �� �����ϴ�. �⺻���� �����մϴ�. BossID: {bossID}");
            StaticBossData = new BossData(BossID.A, 100, "Idle"); // �⺻�� ����
            return false;
        }
    }

    //  ������ ���� ������ (���� ü�� & ID) ����
    public void SaveBossRuntimeData()
    {
        if (BossRuntimeData == null)
        {
            Debug.LogWarning("[BossDataManager] ������ BossRuntimeData�� �����ϴ�.");
            return;
        }

        GM.SaveManager.SaveData(BossRuntimeData);
    }

    //  ������ ���� ������ (���� ü�� & ID) �ε�
    public bool LoadBossRuntimeData()
    {
        if (GM.SaveManager.TryLoadData(out BossRuntimeData data))
        {
            BossRuntimeData = data;
         //   Debug.Log($"[BossDataManager] ���� ��Ÿ�� ������ �ε� �Ϸ�: ID={BossRuntimeData.CurrentBossID}, ü��={BossRuntimeData.CurrentHealth}");
            return true;
        }
        else
        {
            Debug.LogWarning("[BossDataManager] ���� ��Ÿ�� �����Ͱ� ��� �⺻���� �����մϴ�.");
            BossRuntimeData = new BossRuntimeData(BossID.A, StaticBossData.MaxHealth); // �⺻�� ����
            return false;
        }
    }

    #endregion

    #region  ��ü ������ �ε�

    public bool LoadAllData()
    {
        bool runtimeLoaded = LoadBossRuntimeData();

        //  `BossRuntimeData`�� ���������� �ε���� ������ �⺻�� ����
        if (!runtimeLoaded || BossRuntimeData == null)
        {
            BossRuntimeData = new BossRuntimeData(BossID.A, 100);
            Debug.LogWarning("[BossDataManager] BossRuntimeData�� ��� �⺻������ �ʱ�ȭ�Ǿ����ϴ�.");
        }

        bool bossDataLoaded = LoadBossData(BossRuntimeData.CurrentBossID);
        return runtimeLoaded && bossDataLoaded;
    }

    #endregion

    #region  ������ ����

    public void DestroyData()
    {
        BossRuntimeData = new BossRuntimeData(BossID.A, 100);
        StaticBossData = new BossData(BossID.A, 100, "A"); // �⺻�� ����
        GM.SaveManager.SaveData(BossRuntimeData);
        if (GM.SpawnManager != null)
        {
            Boss boss = GM.SpawnManager.GetCurrentBoss();
            if (boss != null)
            {
                boss.ResetBossData();
            }
        }

        Debug.Log("[BossDataManager] ���� ������ �ʱ�ȭ �Ϸ�.");
    }

    #endregion
}
