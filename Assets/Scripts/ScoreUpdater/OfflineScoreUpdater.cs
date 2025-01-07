using UnityEngine;

public class OfflineScoreUpdater : MonoBehaviour
{
    private ScoreUpdater scoreUpdater;

    private void Awake()
    {
        scoreUpdater = GetComponent<ScoreUpdater>();
        if (scoreUpdater == null)
        {
            Debug.LogError("ScoreUpdater�� ã�� �� �����ϴ�!");
        }
    }

    /// <summary>
    /// �������� ���� ���� �߰�
    /// </summary>
    /// <param name="elapsedSeconds">�������� ��� �ð�(��)</param>
    public void CollectOfflineFruits(int elapsedSeconds)
    {
        if (elapsedSeconds <= 0) return;

        // ScoreUpdater�� AddRandomFruits ȣ��
        scoreUpdater.AddRandomFruit();

        Debug.Log($"�������� ���� {elapsedSeconds}�� ���. ���� {elapsedSeconds}�� �������� ���� �Ϸ�.");
    }
}
