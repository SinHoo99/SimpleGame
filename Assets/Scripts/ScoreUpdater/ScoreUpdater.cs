using System;
using System.Linq;
using UnityEngine;

public class ScoreUpdater : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    public event Action<FruitsID> OnFruitCollected;

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

        //  ���� ���� ���
        GM.PlayerDataManager.CollectFruit(fruitID);

        //  ������ �����Ͽ� ��� �ݿ�
        GM.PlayerDataManager.SavePlayerData();

        //  ���� ���� ó�� (Amount ����)
        inventory[fruitID].Amount++;

        //  ��Ȯ�� ������ �����Ͽ� UI ������Ʈ
        GM.UIManager.InventoryManager.FruitUIManager.UpdateOrCreateFruitUI(fruitID, inventory[fruitID].Amount);

        //  �̺�Ʈ�� �������� ȣ���Ͽ� UI ���� �� �����Ͱ� ��Ȯ�ϰ� �ݿ��ǵ��� ��
        OnFruitCollected?.Invoke(fruitID);

        Debug.Log($"[ScoreUpdater] {fruitID} �߰� �Ϸ� - ���� ����: {inventory[fruitID].Amount}");
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
            GM.SpawnManager.SpawnFruitFromPool(selectedFruit.Value);
        }
    }

    /// <summary>
    /// Ȯ�� ��� ���� ���� ����
    /// </summary>
    private FruitsID? GetRandomFruitByProbability()
    {
        var fruits = GM.DataManager.FruitDatas.Values.ToList();

        // ��ü Ȯ���� �� ���
        float totalProbability = fruits.Sum(f => f.Probability);

        // ���� �� ���� (0���� totalProbability ����)
        float randomValue = UnityEngine.Random.Range(0f, totalProbability + 1.0f); // +1.0f�� ���õ��� ���� Ȯ�� �߰�

        float cumulativeProbability = 0f;

        foreach (var fruit in fruits)
        {
            cumulativeProbability += fruit.Probability;
            if (randomValue <= cumulativeProbability)
            {
                GM.AlertManager.ShowAlert($"{fruit.ID} ����");
                return fruit.ID; // ���� ����               
            }
        }

        GM.AlertManager.ShowAlert("������ �����߽��ϴ�.");
        return null; // �ƹ��͵� ���õ��� ����
    }
    #endregion

    #region Ŭ�� �Է� ó��
    public void HandleInput()
    {
        GM.PlaySFX(SFX.Click);

        if (GM.PlayerDataManager.NowPlayerData.PlayerCoin >= 100)
        {
            GM.PlayerStatusUI.PlayerCoin();
            AddRandomFruit(); // Ŭ�� �� ���� ���� ����
            GM.UIManager.InventoryManager.TriggerInventoryUpdate();
        }
        else
        {
            GM.AlertManager.ShowAlert("���� �����մϴ�.");
        }
    }
    #endregion
}
