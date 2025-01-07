using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitItem : MonoBehaviour
{
    public Image fruitIcon;               // ���� ������ �̹���
    public TextMeshProUGUI fruitText;     // ���� �̸� �� ���� �ؽ�Ʈ

    /// <summary>
    /// ���� �������� ������Ʈ�մϴ�.
    /// </summary>
    public void UpdateFruit(string fruitName, int count, Sprite icon)
    {
        fruitText.text = $"{fruitName}: {count}��";
        fruitIcon.sprite = icon;
    }
}
