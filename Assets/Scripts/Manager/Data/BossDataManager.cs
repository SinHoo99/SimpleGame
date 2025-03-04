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

    #region  보스 데이터 저장 및 로드

    //  보스의 정적 데이터 로드 (CSV에서 가져오기)
    public bool LoadBossData(BossID bossID)
    {
        BossData bossData = GM.GetBossData(bossID);
        if (bossData != null)
        {
            StaticBossData = bossData;
          //  Debug.Log($"[BossDataManager] 보스 데이터 로드 완료: ID={bossID}");
            return true;
        }
        else
        {
            Debug.LogWarning($"[BossDataManager] 보스 데이터를 찾을 수 없습니다. 기본값을 설정합니다. BossID: {bossID}");
            StaticBossData = new BossData(BossID.A, 100, "Idle"); // 기본값 설정
            return false;
        }
    }

    //  보스의 동적 데이터 (현재 체력 & ID) 저장
    public void SaveBossRuntimeData()
    {
        if (BossRuntimeData == null)
        {
            Debug.LogWarning("[BossDataManager] 저장할 BossRuntimeData가 없습니다.");
            return;
        }

        GM.SaveManager.SaveData(BossRuntimeData);
    }

    //  보스의 동적 데이터 (현재 체력 & ID) 로드
    public bool LoadBossRuntimeData()
    {
        if (GM.SaveManager.TryLoadData(out BossRuntimeData data))
        {
            BossRuntimeData = data;
         //   Debug.Log($"[BossDataManager] 보스 런타임 데이터 로드 완료: ID={BossRuntimeData.CurrentBossID}, 체력={BossRuntimeData.CurrentHealth}");
            return true;
        }
        else
        {
            Debug.LogWarning("[BossDataManager] 보스 런타임 데이터가 없어서 기본값을 설정합니다.");
            BossRuntimeData = new BossRuntimeData(BossID.A, StaticBossData.MaxHealth); // 기본값 설정
            return false;
        }
    }

    #endregion

    #region  전체 데이터 로드

    public bool LoadAllData()
    {
        bool runtimeLoaded = LoadBossRuntimeData();

        //  `BossRuntimeData`가 정상적으로 로드되지 않으면 기본값 설정
        if (!runtimeLoaded || BossRuntimeData == null)
        {
            BossRuntimeData = new BossRuntimeData(BossID.A, 100);
            Debug.LogWarning("[BossDataManager] BossRuntimeData가 없어서 기본값으로 초기화되었습니다.");
        }

        bool bossDataLoaded = LoadBossData(BossRuntimeData.CurrentBossID);
        return runtimeLoaded && bossDataLoaded;
    }

    #endregion

    #region  데이터 삭제

    public void DestroyData()
    {
        BossRuntimeData = new BossRuntimeData(BossID.A, 100);
        StaticBossData = new BossData(BossID.A, 100, "A"); // 기본값 설정
        GM.SaveManager.SaveData(BossRuntimeData);
        if (GM.SpawnManager != null)
        {
            Boss boss = GM.SpawnManager.GetCurrentBoss();
            if (boss != null)
            {
                boss.ResetBossData();
            }
        }

        Debug.Log("[BossDataManager] 보스 데이터 초기화 완료.");
    }

    #endregion
}
