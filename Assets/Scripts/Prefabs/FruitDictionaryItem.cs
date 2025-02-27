using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FruitDictionaryItem : MonoBehaviour
{
    public FruitsID FruitID; // 과일 ID
    public Image fruitImage; // 과일 아이콘 이미지
    public TextMeshProUGUI FruitsName; // 과일 이름 UI
    public Color uncollectedColor = Color.black; // 수집 전 색상

    private Color _originalColor;

    private void Awake()
    {
        if (fruitImage != null)
        {
            _originalColor = fruitImage.color; // 원래 색상 저장
        }
    }

    public void Setup(FruitsID id, Sprite sprite)
    {
        FruitID = id;

        if (fruitImage != null)
        {
            fruitImage.sprite = sprite;
            fruitImage.color = uncollectedColor; // 기본적으로 수집되지 않은 상태
        }

        if (FruitsName != null)
        {
            FruitsName.text = "???"; // 기본적으로 이름 숨기기
        }
    }

    public void SetCollected(bool collected)
    {
        if (fruitImage != null)
        {
            fruitImage.color = collected ? _originalColor : uncollectedColor;
        }

        // 과일 데이터 가져오기
        var fruitData = GameManager.Instance?.GetFruitsData(FruitID);
        if (FruitsName != null)
        {
            FruitsName.text = (collected && fruitData != null) ? fruitData.Name : "???";
        }
    }
}
