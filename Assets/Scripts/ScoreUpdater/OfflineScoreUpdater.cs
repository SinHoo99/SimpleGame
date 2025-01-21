using System;
using UnityEngine;

public class OfflineScoreUpdater : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    private const int MaxOfflineTimeInSeconds = 7200; // 최대 오프라인 시간 (2시간 = 7200초)

    #region 오프라인 동안 랜덤 과일 수집
    public void CollectOfflineFruits()
    {
        // 오프라인 경과 시간 계산
        DateTime lastCollectedTime = GM.PlayerDataManager.NowPlayerData.LastCollectedTime;
        TimeSpan elapsedTime = DateTime.Now - lastCollectedTime;

        if (elapsedTime.TotalSeconds <= 0)
        {
            Debug.Log("오프라인 경과 시간이 0초 이하입니다. 과일 수집을 건너뜁니다.");
            return;
        }

        // 경과 시간을 최대 2시간으로 제한
        int secondsElapsed = Math.Min((int)elapsedTime.TotalSeconds, MaxOfflineTimeInSeconds);
        Debug.Log($"오프라인 동안 {secondsElapsed}초 경과. 랜덤 과일 수집 시작...");

        // 경과 시간 동안 랜덤 과일 추가
        for (int i = 0; i < secondsElapsed; i++)
        {
            //GameManager.Instance.scoreUpdater.AddRandomFruit(); // 확률 기반 랜덤 과일 추가
        }

        // 마지막 수집 시간 갱신
        GM.PlayerDataManager.NowPlayerData.LastCollectedTime = DateTime.Now;
        Debug.Log($"오프라인 동안 랜덤 과일 {secondsElapsed}개 추가 완료 (최대 2시간 제한).");
    }
    #endregion
}
