using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class UIManager : MonoBehaviour
{
    public GameObject fruitItemPrefab; // 동적으로 생성할 프리팹
    public Transform fruitListParent; // 생성된 UI를 배치할 부모 객체
    public SpriteAtlas fruitAtlas; // 과일 아이콘이 포함된 Sprite Atlas

    private Dictionary<string, FruitItem> _fruitUIItems = new Dictionary<string, FruitItem>();

    public void UpdateFruitCountsUI(Dictionary<string, int> fruitCounts)
    {
        if (fruitItemPrefab == null || fruitListParent == null || fruitAtlas == null)
        {
            Debug.LogWarning("FruitItemPrefab, FruitListParent, 또는 FruitAtlas가 설정되지 않았습니다!");
            return;
        }

        foreach (var fruit in fruitCounts)
        {
            if (fruit.Value > 0) // 0개 이상인 과일만 표시
            {
                if (!_fruitUIItems.ContainsKey(fruit.Key))
                {
                    CreateFruitUI(fruit.Key, fruit.Value);
                }
                else
                {
                    UpdateFruitItem(fruit.Key, fruit.Value);
                }
            }
            else if (_fruitUIItems.ContainsKey(fruit.Key))
            {
                RemoveFruitUI(fruit.Key);
            }
        }
    }

    private void CreateFruitUI(string fruitName, int count)
    {
        GameObject newFruitItemObj = Instantiate(fruitItemPrefab, fruitListParent);

        // FruitItem 컴포넌트 가져오기
        FruitItem fruitItem = newFruitItemObj.GetComponent<FruitItem>();
        if (fruitItem == null)
        {
            Debug.LogError("FruitItemPrefab에 FruitItem 스크립트가 연결되지 않았습니다!");
            Destroy(newFruitItemObj); // 잘못된 프리팹은 즉시 제거
            return;
        }

        Sprite fruitIcon = GetFruitIconFromAtlas(fruitName);
        fruitItem.UpdateFruit(fruitName, count, fruitIcon);

        _fruitUIItems[fruitName] = fruitItem;
    }

    private void UpdateFruitItem(string fruitName, int count)
    {
        if (_fruitUIItems.TryGetValue(fruitName, out FruitItem fruitItem))
        {
            Sprite fruitIcon = GetFruitIconFromAtlas(fruitName);
            fruitItem.UpdateFruit(fruitName, count, fruitIcon);
        }
    }

    private void RemoveFruitUI(string fruitName)
    {
        if (_fruitUIItems.TryGetValue(fruitName, out FruitItem fruitItem))
        {
            Destroy(fruitItem.gameObject);
            _fruitUIItems.Remove(fruitName);
        }
    }

    private Sprite GetFruitIconFromAtlas(string fruitName)
    {
        Sprite sprite = fruitAtlas.GetSprite(fruitName);
        if (sprite == null)
        {
            Debug.LogWarning($"{fruitName}에 해당하는 아이콘이 SpriteAtlas에 없습니다!");
        }
        return sprite;
    }
}
