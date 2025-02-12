using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : PoolObject
{
    private GameManager GM => GameManager.Instance;
    [SerializeField] private FruitsID FruitsID;

    private void OnEnable()
    {
        Debug.Log($"{gameObject.name}�� FruitID ��: {FruitsID}");

        if ((int)FruitsID == 0) // �⺻�� üũ
        {
            Debug.LogWarning($"{gameObject.name}�� FruitID�� �������� �ʾҽ��ϴ�! �ʱ�ȭ�� �ʿ��մϴ�.");
            AssignFruitID();
            return;
        }

        StartCoroutine(UpdateCoinCoroutine());
    }

    private IEnumerator UpdateCoinCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1�ʸ��� ����
            if (GM.dataManager.FriutDatas.TryGetValue(FruitsID, out var fruitsData))
            {
                GM.playerDataManager.NowPlayerData.PlayerCoin += fruitsData.Price;
                GM.uiManager.TriggerInventoryUpdate();
            }
            else
            {
                Debug.LogWarning($"�ش� ���� ID({FruitsID})�� ���� �����͸� ã�� �� �����ϴ�.");
            }
        }
    }
    public void AssignFruitID()
    {
        if ((int)FruitsID != 0) return; // **�̹� ������ ��� ����**

        string prefabName = gameObject.name.Replace("(Clone)", "").Trim();
        Debug.Log($"[AssignFruitID] {gameObject.name}�� PrefabName: {prefabName}");

        if (GM.dataManager.FriutDatas == null)
        {
            Debug.LogError("[AssignFruitID] GM.DataManager.FriutDatas�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        foreach (var fruitData in GM.dataManager.FriutDatas.Values)
        {
            if (fruitData.Name.Trim() == prefabName)
            {
                FruitsID = fruitData.ID;
                Debug.Log($"[AssignFruitID] {gameObject.name}�� FruitID�� {FruitsID}�� ������.");
                return;
            }
        }

        Debug.LogError($"[AssignFruitID] {gameObject.name}�� FruitsID �ڵ� �Ҵ� ����! CSV���� {prefabName}�� ã�� �� �����ϴ�.");
    }
}

