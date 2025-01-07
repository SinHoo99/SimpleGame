using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitItem : MonoBehaviour
{
    public Image fruitIcon;               // 과일 아이콘 이미지
    public TextMeshProUGUI fruitText;     // 과일 이름 및 개수 텍스트

    /// <summary>
    /// 과일 아이템을 업데이트합니다.
    /// </summary>
    public void UpdateFruit(string fruitName, int count, Sprite icon)
    {
        fruitText.text = $"{fruitName}: {count}개";
        fruitIcon.sprite = icon;
    }
}
