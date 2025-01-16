using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private float timeAccumulator = 0f; // �ð� ���� ����
    private const float collectionInterval = 1f; // 1�ʸ��� ���� ����

    private void Update()
    {
        HandleInput(); // Ŭ�� �Է� ó��
       // CollectFruitsOverTime(); // �ð��� ���� ���� ����
    }

    /// <summary>
    /// Ư�� ������ �߰��մϴ�.
    /// </summary>
    public void AddFruits(FruitsID fruitID, int count)
    {
        var inventory = GameManager.Instance.NowPlayerData.Inventory;

        if (!inventory.ContainsKey(fruitID))
        {
            Debug.LogWarning($"{fruitID}�� Inventory�� �������� �ʽ��ϴ�.");
            return;
        }

        // ���� ���� �߰�
        inventory[fruitID].Amount += count;
        //Debug.Log($"{fruitID} �߰���. ���� ����: {inventory[fruitID].Amount}");

        // UI ������Ʈ
        GameManager.Instance.UpdateUIWithInventory();
    }

    /// <summary>
    /// ���� Ȯ�� ��� ���� �߰�
    /// </summary>
    public void AddRandomFruit()
    {
        // Ȯ�� ������� ���� ����
        FruitsID selectedFruit = GetRandomFruitByProbability();
        AddFruits(selectedFruit, 1); // ���õ� ������ 1�� �߰�
    }

    /// <summary>
    /// Ȯ�� ��� ���� ���� ����
    /// </summary>
    private FruitsID GetRandomFruitByProbability()
    {
        float totalProbability = 0;
        foreach (var fruit in GameManager.Instance.DataManager.FriutDatas.Values)
        {
            totalProbability += fruit.Probability;
        }

        float randomValue = Random.Range(0f, totalProbability);
        float cumulativeProbability = 0;

        foreach (var fruit in GameManager.Instance.DataManager.FriutDatas.Values)
        {
            cumulativeProbability += fruit.Probability;
            if (randomValue <= cumulativeProbability)
            {
                return fruit.ID;
            }
        }

        Debug.LogWarning("Ȯ�� ��� ����: �⺻�� ��ȯ");
        return FruitsID.Apple; // �⺻��
    }

    /// <summary>
    /// Ŭ�� �Է� ó��
    /// </summary>
    private void HandleInput()
    {
        if (IsInputDetected())
        {
            AddRandomFruit(); // Ŭ�� �� ���� ���� ����
        }
    }

    /// <summary>
    /// �Է� ����
    /// </summary>
    /// <returns>�Է� ����</returns>
    private bool IsInputDetected()
    {
        if (Input.GetMouseButtonDown(0)) // ���콺 Ŭ�� ����
        {
            return true;
        }

#if UNITY_EDITOR
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            return true;
        }
#endif
        return false;
    }

    /// <summary>
    /// �ð��� ���� ���� ����
    /// </summary>
    private void CollectFruitsOverTime()
    {
        timeAccumulator += Time.deltaTime;

        if (timeAccumulator >= collectionInterval)
        {
            int collectCount = Mathf.FloorToInt(timeAccumulator / collectionInterval);

            for (int i = 0; i < collectCount; i++)
            {
                AddRandomFruit();
            }

            timeAccumulator %= collectionInterval; // �ʰ��� �ð��� ����
        }
    }
}
