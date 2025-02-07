using System.Linq;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    private float timeAccumulator = 0f; // �ð� ���� ����
    private const float collectionInterval = 1f; // 1�ʸ��� ���� ����


    #region ���� ���� ����
    /// <summary>
    /// Ư�� ������ �߰��մϴ�.
    /// </summary>
    public void AddFruits(FruitsID fruitID)
    {
        var inventory = GM.PlayerDataManager.NowPlayerData.Inventory;

        if (!inventory.ContainsKey(fruitID))
        {
            Debug.LogWarning($"{fruitID}�� Inventory�� �������� �ʽ��ϴ�.");
            return;
        }

        // ���� ���� ó�� (Amount ���� ��)
        inventory[fruitID].Amount++;

        // ��� ������ ���� ID�� UI ������Ʈ
        GameManager.Instance.UIManager.UpdateOrCreateFruitUI(fruitID, 1);

    }

    /// <summary>
    /// ���� Ȯ�� ��� ���� �߰�
    /// </summary>
    public void AddRandomFruit()
    {
        FruitsID? selectedFruit = GetRandomFruitByProbability();

        if (selectedFruit.HasValue)
        {
            AddFruits(selectedFruit.Value); // null�� �ƴ� ��츸 AddFruits ȣ��
            GameManager.Instance.SpawnManager.SpawnFruitFromPool(selectedFruit.Value);
        }
        else
        {
            Debug.Log("�ƹ� ���ϵ� ���õ��� �ʾҽ��ϴ�.");
        }
    }

    /// <summary>
    /// Ȯ�� ��� ���� ���� ����
    /// </summary>
    private FruitsID? GetRandomFruitByProbability()
    {
        var fruits = GameManager.Instance.DataManager.FriutDatas.Values.ToList();

        // ��ü Ȯ���� �� ���
        float totalProbability = fruits.Sum(f => f.Probability);

        // ���� �� ���� (0���� totalProbability ����)
        float randomValue = Random.Range(0f, totalProbability + 1.0f); // +1.0f�� ���õ��� ���� Ȯ�� �߰�

        float cumulativeProbability = 0f;

        foreach (var fruit in fruits)
        {
            cumulativeProbability += fruit.Probability;
            if (randomValue <= cumulativeProbability)
            {
                return fruit.ID; // ���� ����
            }
        }

        // ���õ��� ���� Ȯ��
        Debug.Log("�ƹ� ���ϵ� ���õ��� �ʾҽ��ϴ�.");
        return null; // �ƹ��͵� ���õ��� ����
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
    #endregion

    public void AddCoin()
    {
        GM.PlayerDataManager.NowPlayerData.PlayerCoin += 1;
    }

    /// <summary>
    /// Ŭ�� �Է� ó��
    /// </summary>
    public void HandleInput()
    {
        AddRandomFruit(); // Ŭ�� �� ���� ���� ����
        AddCoin();
        GameManager.Instance.UIManager.TriggerInventoryUpdate();
        GM.PlayerDataManager.NowPlayerData.PlayerCoin -= 100;
    }
}
