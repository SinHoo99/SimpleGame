using System.Collections;
using UnityEngine;

public class CoinUpdater : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    public FruitsID FruitID;

    private void Awake()
    {
        AssignFruitID();
    }
    private void OnEnable()
    {
        Debug.Log($"{gameObject.name}�� FruitID ��: {FruitID}");

        if ((int)FruitID == 0) // �⺻�� üũ
        {
            Debug.LogWarning($"{gameObject.name}�� FruitID�� �������� �ʾҽ��ϴ�! �ʱ�ȭ�� �ʿ��մϴ�.");
            return;
        }

        StartCoroutine(UpdateCoinCoroutine());
    }

    public void AssignFruitID()
    {
        if ((int)FruitID == 0) 
        {
            string prefabName = gameObject.name.Replace("(Clone)", "").Trim();

            // CSV �����Ϳ��� prefabName�� �ش��ϴ� FruitsID ã��
            foreach (var fruitData in GM.DataManager.FriutDatas.Values)
            {
                if (fruitData.Name == prefabName)
                {
                    FruitID = fruitData.ID;
                    Debug.Log($"[CoinUpdater] {gameObject.name}�� FruitID�� CSV �����Ϳ��� {FruitID}�� ������.");
                    return;
                }
            }
            Debug.LogError($"[CoinUpdater] {gameObject.name}�� FruitsID �ڵ� �Ҵ� ����! CSV���� {prefabName}��(��) ã�� �� �����ϴ�.");
        }
    }

    private IEnumerator UpdateCoinCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f); // 1�ʸ��� ����
            if (GM.DataManager.FriutDatas.TryGetValue(FruitID, out var fruitsData))
            {
                GM.PlayerDataManager.NowPlayerData.PlayerCoin += fruitsData.Price;
                GM.UIManager.TriggerInventoryUpdate();
            }
            else
            {
                Debug.LogWarning($"�ش� ���� ID({FruitID})�� ���� �����͸� ã�� �� �����ϴ�.");
            }
        }
    }
}
