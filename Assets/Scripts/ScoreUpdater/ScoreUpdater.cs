using System.Linq;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

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
        GameManager.Instance.UIManager.InventoryManager.FruitUIManager.UpdateOrCreateFruitUI(fruitID, 1);
    }

    /// ���� Ȯ�� ��� ���� �߰�
    public void AddRandomFruit()
    {
        FruitsID? selectedFruit = GetRandomFruitByProbability();

        if (selectedFruit.HasValue)
        {
            AddFruits(selectedFruit.Value); // null�� �ƴ� ��츸 AddFruits ȣ��
            GameManager.Instance.SpawnManager.SpawnFruitFromPool(selectedFruit.Value);
        }
    }
    /// Ȯ�� ��� ���� ���� ����
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
        return null; // �ƹ��͵� ���õ��� ����
    }
    #endregion

    #region Ŭ�� �Է� ó��
    public void HandleInput()
    {
        if (GM.PlayerDataManager.NowPlayerData.PlayerCoin >= 100)
        {
            GM.PlayerDataManager.NowPlayerData.PlayerCoin -= 100;
            GM.PlayerStatusUI.PlayerCoin();
            AddRandomFruit(); // Ŭ�� �� ���� ���� ����
            GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();
        }
        else
        {
            GM.AlertManager.ShowAlert("���� �����մϴ�.");
        }
  
    }
    #endregion

}