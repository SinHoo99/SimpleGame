using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitItem : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    public Button fruitButton;
    public TextMeshProUGUI fruitText;
    private FruitsID fruitID;


    /// ���� �������� ������Ʈ�ϰ� fruitID�� �����ϴ� �Լ�
    public void UpdateFruit(FruitsID id, int count, Sprite icon)
    {
        fruitID = id;

        // ��ư �̹��� ����
        Image buttonImage = fruitButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = icon;
        }

        // ���� ���� �ؽ�Ʈ ������Ʈ
        fruitText.text = $"{count}��";
    }


    /// ���� ��ư Ŭ�� �� ����Ǵ� �̺�Ʈ
    public void OnFruitButtonClicked()
    {
        GameManager.Instance.UIManager.InventoryManager.FruitUIManager.OnFruitSelected(fruitID);

        // ObjectPool���� ���� ������Ʈ ��������
        PoolObject objToReturn = FindActiveFruit();

        if (objToReturn != null)
        {
            GameManager.Instance.ObjectPool.ReturnObject(fruitID.ToString(), objToReturn);

            bool success = SubtractFruitAndCalculateCoins(
                fruitID,1
                ,GameManager.Instance.DataManager.FriutDatas
            );

            if (success)
            {
                Debug.Log($"{fruitID} ��ȯ �Ϸ�, ���� ȹ�� �� UI ���� �Ϸ�");
            }
            else
            {
                Debug.LogWarning($"���� ������({fruitID})�� �������� �ʾ� ������ ������ �� �����ϴ�.");
            }
        }
        else
        {
            Debug.LogWarning($"{fruitID}�� �ش��ϴ� Ȱ��ȭ�� ������Ʈ�� �����ϴ�.");
        }
        GameManager.Instance.UIManager.InventoryManager.TriggerInventoryUpdate();
    }
    /// ObjectPool���� ���� Ȱ��ȭ�� ���� ������Ʈ�� ã�� ��ȯ
    private PoolObject FindActiveFruit()
    {
        if (!GameManager.Instance.ObjectPool.PoolDictionary.TryGetValue(fruitID.ToString(), out List<PoolObject> fruitList))
        {
            return null;
        }

        foreach (var obj in fruitList)
        {
            if (obj.gameObject.activeInHierarchy)
            {
                return obj;
            }
        }

        return null;
    }
    public bool SubtractFruitAndCalculateCoins(FruitsID fruitID, int amount, Dictionary<FruitsID, FruitsData> fruitDataDictionary)
    {
        if (!fruitDataDictionary.TryGetValue(fruitID, out var fruitData))
        {
            Debug.LogWarning($"���� ������({fruitID})�� ã�� �� �����ϴ�.");
            return false;
        }

        // ���� ���� ����
        if (GM.PlayerDataManager.NowPlayerData.Inventory.TryGetValue(fruitID, out var collectedFruit))
        {
            collectedFruit.Amount -= amount;
        }
        else
        {
            GM.PlayerDataManager.NowPlayerData.Inventory[fruitID] = new CollectedFruitData { ID = fruitID, Amount = amount };
        }
        return true;
    }
}
