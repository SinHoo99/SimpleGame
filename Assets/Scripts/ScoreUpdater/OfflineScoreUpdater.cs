using UnityEngine;

public class OfflineScoreUpdater : MonoBehaviour
{
    private ScoreUpdater scoreUpdater;

    private void Awake()
    {
        scoreUpdater = GetComponent<ScoreUpdater>();
        if (scoreUpdater == null)
        {
            Debug.LogError("ScoreUpdater를 찾을 수 없습니다!");
        }
    }

    /// <summary>
    /// 오프라인 동안 과일 추가
    /// </summary>
    /// <param name="elapsedSeconds">오프라인 경과 시간(초)</param>
    public void CollectOfflineFruits(int elapsedSeconds)
    {
        if (elapsedSeconds <= 0) return;

        // ScoreUpdater의 AddRandomFruits 호출
        scoreUpdater.AddRandomFruit();

        Debug.Log($"오프라인 동안 {elapsedSeconds}초 경과. 과일 {elapsedSeconds}개 랜덤으로 수집 완료.");
    }
}
