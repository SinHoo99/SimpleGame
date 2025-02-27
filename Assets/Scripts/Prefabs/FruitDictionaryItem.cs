using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitDictionaryItem : MonoBehaviour
{
    public FruitsID FruitID; // ���� ID
    public Image fruitImage; // ���� ������ �̹���
    public TextMeshProUGUI FruitsName; // ���� �̸� UI
    public Color uncollectedColor = Color.black; // ���� �� ����

    private Color _originalColor;

    private void Awake()
    {
        if (fruitImage != null)
        {
            _originalColor = fruitImage.color; // ���� ���� ����
        }
    }

    public void Setup(FruitsID id, Sprite sprite)
    {
        FruitID = id;

        if (fruitImage != null)
        {
            fruitImage.sprite = sprite;
            fruitImage.color = uncollectedColor; // �⺻������ �������� ���� ����
        }

        if (FruitsName != null)
        {
            FruitsName.text = "???"; // �⺻������ �̸� �����
        }
    }

    public void SetCollected(bool collected)
    {
        if (fruitImage != null)
        {
            fruitImage.color = collected ? _originalColor : uncollectedColor;
        }

        // ���� ������ ��������
        var fruitData = GameManager.Instance?.GetFruitsData(FruitID);
        if (FruitsName != null)
        {
            FruitsName.text = (collected && fruitData != null) ? fruitData.Name : "???";
        }
    }
}
