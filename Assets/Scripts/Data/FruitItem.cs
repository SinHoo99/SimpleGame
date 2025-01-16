using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitItem : MonoBehaviour
{
    public Button fruitButton;
    public TextMeshProUGUI fruitText;
    private FruitsID fruitID;

    // 과일 아이템을 업데이트하고 fruitID를 초기화
    public void UpdateFruit(FruitsID id, int count, Sprite icon)
    {
        // fruitID 초기화
        fruitID = id;

        // 버튼의 이미지를 설정
        Image buttonImage = fruitButton.GetComponent<Image>();
        if (buttonImage != null)
        {
            buttonImage.sprite = icon;
        }

        // 텍스트 업데이트
        fruitText.text = $"{count}개";
    }

    // 버튼 클릭 이벤트
    private void Start()
    {
        fruitButton.onClick.RemoveAllListeners();
        fruitButton.onClick.AddListener(OnFruitButtonClicked);
    }

    private void OnFruitButtonClicked()
    {
        // UIManager에 현재 과일 ID 전달
        GameManager.Instance.uiManager.OnFruitSelected(fruitID);

        //GameManager.Instance.NowPlayerData.Inventory.
    }
}
