using System.Collections.Generic;
using UnityEngine;

public class FruitUIManager : MonoBehaviour
{
    public GameObject fruitItemPrefab;
    public Transform fruitListParent;

    private Dictionary<FruitsID, FruitsData> _fruitData;
    private readonly Dictionary<FruitsID, FruitItem> _fruitUIItems = new();
    private readonly Dictionary<FruitsID, Sprite> _fruitSprites = new();

    public void SetFruitData(Dictionary<FruitsID, FruitsData> fruitData)
    {
        if (fruitData == null) return;

        _fruitData = fruitData;
        _fruitSprites.Clear();

        foreach (var (id, data) in _fruitData)
        {
            _fruitSprites[id] = data.Image;
        }
    }

    public void UpdateFruitCountsUI(Dictionary<FruitsID, int> fruitCounts)
    {
        foreach (var (fruitID, count) in fruitCounts)
        {
            if (count > 0) UpdateOrCreateFruitUI(fruitID, count);
            else RemoveFruitUI(fruitID);
        }
    }

    public void UpdateOrCreateFruitUI(FruitsID id, int count)
    {
        if (_fruitUIItems.TryGetValue(id, out var fruitItem))
        {
            fruitItem.UpdateFruit(id, count, GetFruitImage(id));
        }
        else
        {
            CreateFruitUI(id, count);
        }
    }

    public void CreateFruitUI(FruitsID id, int count)
    {
        if (_fruitData == null || !_fruitData.TryGetValue(id, out var fruitData))
        {
            Debug.LogWarning($"���� ������({id})�� ã�� �� �����ϴ�.");
            return;
        }

        var fruitItemObject = Instantiate(fruitItemPrefab, fruitListParent);
        if (!fruitItemObject.TryGetComponent<FruitItem>(out var fruitItem))
        {
            Debug.LogError("FruitItemPrefab�� FruitItem ��ũ��Ʈ�� ������� �ʾҽ��ϴ�.");
            Destroy(fruitItemObject);
            return;
        }

        fruitItem.UpdateFruit(id, count, fruitData.Image);
        _fruitUIItems[id] = fruitItem;
    }

    public void RemoveFruitUI(FruitsID id)
    {
        if (_fruitUIItems.TryGetValue(id, out var fruitItem))
        {
            Destroy(fruitItem.gameObject);
            _fruitUIItems.Remove(id);
        }
    }

    public void OnFruitSelected(FruitsID selectedFruitID)
    {
        if (_fruitData == null || !_fruitData.TryGetValue(selectedFruitID, out var fruitData))
        {
            Debug.LogWarning($"ID {selectedFruitID}�� �ش��ϴ� ���� �����͸� ã�� �� �����ϴ�.");
            return;
        }

        Debug.Log($"���� ���õ�: {selectedFruitID}");
        // ���⿡ ���õ� ������ ó���ϴ� �߰� ������ ���� �� ����.
    }

    public Sprite GetFruitImage(FruitsID id)
    {
        return _fruitSprites.TryGetValue(id, out var sprite) ? sprite : null;
    }
}
