using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.U2D;

public class UIManager : MonoBehaviour
{
    public GameObject fruitItemPrefab; // �������� ������ ������
    public Transform fruitListParent; // ������ UI�� ��ġ�� �θ� ��ü
    public SpriteAtlas fruitAtlas; // ���� �������� ���Ե� Sprite Atlas

    private Dictionary<string, FruitItem> _fruitUIItems = new Dictionary<string, FruitItem>();

    public void UpdateFruitCountsUI(Dictionary<string, int> fruitCounts)
    {
        if (fruitItemPrefab == null || fruitListParent == null || fruitAtlas == null)
        {
            Debug.LogWarning("FruitItemPrefab, FruitListParent, �Ǵ� FruitAtlas�� �������� �ʾҽ��ϴ�!");
            return;
        }

        foreach (var fruit in fruitCounts)
        {
            if (fruit.Value > 0) // 0�� �̻��� ���ϸ� ǥ��
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

        // FruitItem ������Ʈ ��������
        FruitItem fruitItem = newFruitItemObj.GetComponent<FruitItem>();
        if (fruitItem == null)
        {
            Debug.LogError("FruitItemPrefab�� FruitItem ��ũ��Ʈ�� ������� �ʾҽ��ϴ�!");
            Destroy(newFruitItemObj); // �߸��� �������� ��� ����
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
            Debug.LogWarning($"{fruitName}�� �ش��ϴ� �������� SpriteAtlas�� �����ϴ�!");
        }
        return sprite;
    }
}
