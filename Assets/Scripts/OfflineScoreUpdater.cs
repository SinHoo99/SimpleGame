using System;
using System.Collections.Generic;
using UnityEngine;

public class OfflineScoreUpdater : MonoBehaviour
{
    private Dictionary<string, int> fruitCounts; // 과일 개수 관리
    private UIManager uiManager;

    private void Awake()
    {
        // UIManager 찾기
        uiManager = GetComponent<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager를 찾을 수 없습니다!");
        }
    }

    public void Initialize(GameData gameData)
    {
        fruitCounts = gameData.fruitCounts ?? new Dictionary<string, int>();

        if (!string.IsNullOrEmpty(gameData.lastSaveTime))
        {
            if (DateTime.TryParse(gameData.lastSaveTime, out DateTime lastSaveTime))
            {
                TimeSpan timeDifference = DateTime.Now - lastSaveTime;

                Debug.Log($"lastSaveTime: {lastSaveTime}");
                Debug.Log($"현재 시간: {DateTime.Now}");
                Debug.Log($"경과 시간(초): {timeDifference.TotalSeconds}");

                // 경과 시간 제한: 하루(86400초)로 제한
                int elapsedSeconds = Mathf.Min((int)timeDifference.TotalSeconds, 86400);
                if (elapsedSeconds > 0)
                {
                    CollectOfflineFruits(elapsedSeconds);
                }
            }
            else
            {
                Debug.LogWarning("lastSaveTime을 파싱할 수 없습니다. 기본 값으로 초기화합니다.");
            }
        }

        Debug.Log("OfflineScoreUpdater 초기화 완료.");
    }

    public void CollectOfflineFruits(int elapsedSeconds)
    {
        if (elapsedSeconds <= 0) return;

        // 오프라인 동안 경과한 시간만큼 과일 수집
        List<string> fruitKeys = new List<string>(fruitCounts.Keys);
        if (fruitKeys.Count == 0)
        {
            Debug.LogWarning("과일 목록이 비어 있습니다. 과일 수집을 건너뜁니다.");
            return;
        }

        for (int i = 0; i < elapsedSeconds; i++)
        {
            string randomFruit = fruitKeys[UnityEngine.Random.Range(0, fruitKeys.Count)];
            fruitCounts[randomFruit]++;
        }
        Debug.Log($"오프라인 동안 {elapsedSeconds}초 경과. 과일 {elapsedSeconds}개 랜덤으로 수집 완료.");

        // UI 갱신
        uiManager?.UpdateFruitCountsUI(fruitCounts);
    }

    public Dictionary<string, int> GetFruitCounts()
    {
        return new Dictionary<string, int>(fruitCounts);
    }
}