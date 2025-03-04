using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitItem : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    public Button fruitButton;
    public TextMeshProUGUI fruitText;
    private FruitsID fruitID;

    /// <summary>
    /// ���� �������� ������Ʈ�ϰ� fruitID�� �����ϴ� �Լ�
    /// </summary>
    public void UpdateFruit(FruitsID id, int count, Sprite icon)
    {
        fruitID = id;

        // ��ư �̹��� ����
        if (fruitButton.TryGetComponent<Image>(out var buttonImage))
        {
            buttonImage.sprite = icon;
        }

        // ���� ���� �ؽ�Ʈ ������Ʈ
        fruitText.text = $"{count}�� / �ǸŰ��� : {GM.GetFruitsData(id).Price}";
    }

    /// ���� ��ư Ŭ�� �� ����Ǵ� �̺�Ʈ
    public void OnFruitButtonClicked()
    {
        GM.UIManager.InventoryManager.FruitUIManager.OnFruitSelected(fruitID);

        // ObjectPool���� ���� ������Ʈ ��������
        var objToReturn = GM.ObjectPool.FindActiveObject(fruitID.ToString());

        if (objToReturn != null)
        {
            GM.ObjectPool.ReturnObject(fruitID.ToString(), objToReturn);

            if (SubtractFruitAndCalculateCoins(fruitID, 1))
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

        GM.UIManager.InventoryManager.TriggerInventoryUpdate();
    }

    /// ���� ���� ���� �� ���� ���
    public bool SubtractFruitAndCalculateCoins(FruitsID fruitID, int amount)
    {
        var fruitData = GM.GetFruitsData(fruitID);
        if (fruitData == null)
        {
            Debug.LogWarning($"���� ������({fruitID})�� ã�� �� �����ϴ�.");
            return false;
        }

        var collectedFruit = GM.GetCollectedFruitData(fruitID);
        if (collectedFruit.Amount < amount)
        {
            Debug.LogWarning($"{fruitID}�� ������ �����Ͽ� ������ �� �����ϴ�.");
            return false;
        }

        collectedFruit.Amount -= amount;
        GM.PlayerDataManager.NowPlayerData.PlayerCoin += fruitData.Price;
        return true;
    }
}
