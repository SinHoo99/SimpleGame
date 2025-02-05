using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitItem : MonoBehaviour
{
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
        Image buttonImage = fruitButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = icon;
        }

        // ���� ���� �ؽ�Ʈ ������Ʈ
        fruitText.text = $"{count}��";
    }

    private void Start()
    {
        fruitButton.onClick.RemoveAllListeners();
        fruitButton.onClick.AddListener(OnFruitButtonClicked);
    }

    /// <summary>
    /// ���� ��ư Ŭ�� �� ����Ǵ� �̺�Ʈ
    /// </summary>
    private void OnFruitButtonClicked()
    {
        GameManager.Instance.UIManager.OnFruitSelected(fruitID);

        // ObjectPool���� ���� ������Ʈ ��������
        PoolObject objToReturn = FindActiveFruit();

        if (objToReturn != null)
        {
            GameManager.Instance.ObjectPool.ReturnObject(fruitID.ToString(), objToReturn);

            bool success = GameManager.Instance.PlayerDataManager.NowPlayerData.AddFruitAndCalculateCoins(
                fruitID,1
                ,GameManager.Instance.DataManager.FriutDatas
            );

            if (success)
            {
                GameManager.Instance.UIManager.UpdateUIWithInventory();

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

    }

    /// <summary>
    /// ObjectPool���� ���� Ȱ��ȭ�� ���� ������Ʈ�� ã�� ��ȯ
    /// </summary>
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
}
