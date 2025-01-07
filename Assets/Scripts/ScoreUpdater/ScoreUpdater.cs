using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private Dictionary<FriutsID, NowFruitsData> collectionList; // ���� ���

    private void Awake()
    {
        InitializeCollectionList();
    }
    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Debug.Log("��ġ �Է� ������");
            AddRandomFruit(); // ���� ���� ���� ���� ȣ��
        }

#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("���콺 Ŭ�� ������");
            AddRandomFruit(); // ���� ���� ���� ���� ȣ��
        }
#endif
    }
    /// <summary>
    /// Ư�� ������ �߰��մϴ�.
    /// </summary>
    public void AddFruits(FriutsID fruitID, int count)
    {
        if (!collectionList.ContainsKey(fruitID))
        {
            Debug.LogWarning($"{fruitID}�� ���� ��Ͽ� �����ϴ�.");
            return;
        }

        // ���� ���� ����
        collectionList[fruitID].Amount += count;
        Debug.Log($"{fruitID} �߰���, ���� ����: {collectionList[fruitID].Amount}");

        // UI ������Ʈ
        GameManager.Instance.uiManager.UpdateFruitCountsUI(
            GameManager.Instance.NowPlayerData.Inventory.ToDictionary(kv => kv.Key, kv => kv.Value.Amount)
        );
    }

    /// <summary>
    /// ���� Ȯ�� ��� ���� �߰�
    /// </summary>
    public void AddRandomFruit()
    {
        // Ȯ�� ������� ���� ����
        FriutsID selectedFruit = GetRandomFruitByProbability();
        AddFruits(selectedFruit, 1); // ���õ� ������ 1�� �߰�
    }

    /// <summary>
    /// Ȯ�� ��� ���� ���� ����
    /// </summary>
    private FriutsID GetRandomFruitByProbability()
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
        return FriutsID.Apple; // �⺻��
    }

    /// <summary>
    /// ���� ��� �ʱ�ȭ
    /// </summary>
    private void InitializeCollectionList()
    {
        collectionList = GameManager.Instance.NowPlayerData.Inventory;

        foreach (FriutsID id in System.Enum.GetValues(typeof(FriutsID)))
        {
            collectionList.TryAdd(id, new NowFruitsData { ID = id, Amount = 0 });
        }

        Debug.Log("���� ��� �ʱ�ȭ �Ϸ�");
    }
}
