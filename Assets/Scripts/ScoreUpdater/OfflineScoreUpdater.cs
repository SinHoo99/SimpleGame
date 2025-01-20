using System;
using UnityEngine;

public class OfflineScoreUpdater : MonoBehaviour
{
    private const int MaxOfflineTimeInSeconds = 7200; // �ִ� �������� �ð� (2�ð� = 7200��)

    /// <summary>
    /// �������� ���� ���� ���� ����
    /// </summary>
    public void CollectOfflineFruits()
    {
        // �������� ��� �ð� ���
        DateTime lastCollectedTime = GameManager.Instance.NowPlayerData.LastCollectedTime;
        TimeSpan elapsedTime = DateTime.Now - lastCollectedTime;

        if (elapsedTime.TotalSeconds <= 0)
        {
            Debug.Log("�������� ��� �ð��� 0�� �����Դϴ�. ���� ������ �ǳʶݴϴ�.");
            return;
        }

        // ��� �ð��� �ִ� 2�ð����� ����
        int secondsElapsed = Math.Min((int)elapsedTime.TotalSeconds, MaxOfflineTimeInSeconds);
        Debug.Log($"�������� ���� {secondsElapsed}�� ���. ���� ���� ���� ����...");

        // ��� �ð� ���� ���� ���� �߰�
        for (int i = 0; i < secondsElapsed; i++)
        {
            //GameManager.Instance.scoreUpdater.AddRandomFruit(); // Ȯ�� ��� ���� ���� �߰�
        }

        // ������ ���� �ð� ����
        GameManager.Instance.NowPlayerData.LastCollectedTime = DateTime.Now;
        Debug.Log($"�������� ���� ���� ���� {secondsElapsed}�� �߰� �Ϸ� (�ִ� 2�ð� ����).");
    }
}
