using System;
using System.Collections.Generic;
using UnityEngine;

public class OfflineScoreUpdater : MonoBehaviour
{
    private Dictionary<string, int> fruitCounts; // ���� ���� ����
    private UIManager uiManager;

    private void Awake()
    {
        // UIManager ã��
        uiManager = GetComponent<UIManager>();
        if (uiManager == null)
        {
            Debug.LogError("UIManager�� ã�� �� �����ϴ�!");
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
                Debug.Log($"���� �ð�: {DateTime.Now}");
                Debug.Log($"��� �ð�(��): {timeDifference.TotalSeconds}");

                // ��� �ð� ����: �Ϸ�(86400��)�� ����
                int elapsedSeconds = Mathf.Min((int)timeDifference.TotalSeconds, 86400);
                if (elapsedSeconds > 0)
                {
                    CollectOfflineFruits(elapsedSeconds);
                }
            }
            else
            {
                Debug.LogWarning("lastSaveTime�� �Ľ��� �� �����ϴ�. �⺻ ������ �ʱ�ȭ�մϴ�.");
            }
        }

        Debug.Log("OfflineScoreUpdater �ʱ�ȭ �Ϸ�.");
    }

    public void CollectOfflineFruits(int elapsedSeconds)
    {
        if (elapsedSeconds <= 0) return;

        // �������� ���� ����� �ð���ŭ ���� ����
        List<string> fruitKeys = new List<string>(fruitCounts.Keys);
        if (fruitKeys.Count == 0)
        {
            Debug.LogWarning("���� ����� ��� �ֽ��ϴ�. ���� ������ �ǳʶݴϴ�.");
            return;
        }

        for (int i = 0; i < elapsedSeconds; i++)
        {
            string randomFruit = fruitKeys[UnityEngine.Random.Range(0, fruitKeys.Count)];
            fruitCounts[randomFruit]++;
        }
        Debug.Log($"�������� ���� {elapsedSeconds}�� ���. ���� {elapsedSeconds}�� �������� ���� �Ϸ�.");

        // UI ����
        uiManager?.UpdateFruitCountsUI(fruitCounts);
    }

    public Dictionary<string, int> GetFruitCounts()
    {
        return new Dictionary<string, int>(fruitCounts);
    }
}