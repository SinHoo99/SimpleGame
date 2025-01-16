using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitItem : MonoBehaviour
{
    public Button fruitButton;
    public TextMeshProUGUI fruitText;
    private FruitsID fruitID;

    // ���� �������� ������Ʈ�ϰ� fruitID�� �ʱ�ȭ
    public void UpdateFruit(FruitsID id, int count, Sprite icon)
    {
        // fruitID �ʱ�ȭ
        fruitID = id;

        // ��ư�� �̹����� ����
        Image buttonImage = fruitButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = icon;
        }

        // �ؽ�Ʈ ������Ʈ
        fruitText.text = $"{count}��";
    }

    // ��ư Ŭ�� �̺�Ʈ
    private void Start()
    {
        fruitButton.onClick.RemoveAllListeners();
        fruitButton.onClick.AddListener(OnFruitButtonClicked);
    }

    private void OnFruitButtonClicked()
    {
        // UIManager�� ���� ���� ID ����
        GameManager.Instance.uiManager.OnFruitSelected(fruitID);

        //GameManager.Instance.NowPlayerData.Inventory.
    }
}
