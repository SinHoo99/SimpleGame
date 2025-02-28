using System.Collections.Generic;
using UnityEngine;

public class FruitUIManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    public GameObject fruitItemPrefab;
    public Transform fruitListParent;

    private readonly Dictionary<FruitsID, FruitItem> _fruitUIItems = new();
    private readonly Dictionary<FruitsID, Sprite> _fruitSprites = new();

    public void SetFruitData(Dictionary<FruitsID, FruitsData> fruitData)
    {
        if (fruitData == null) return;

        _fruitSprites.Clear();
        foreach (var (id, data) in fruitData)
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
        if (_fruitUIItems.ContainsKey(id))
        {
            _fruitUIItems[id].UpdateFruit(id, count, GetFruitImage(id));
        }
        else
        {
            CreateFruitUI(id, count);
        }
    }


    public void CreateFruitUI(FruitsID id, int count)
    {
        var fruitData = GM.GetFruitsData(id);
        if (fruitData == null)
        {
            Debug.LogWarning($"과일 데이터({id})를 찾을 수 없습니다.");
            return;
        }

        var fruitItemObject = Instantiate(fruitItemPrefab, fruitListParent);
        if (!fruitItemObject.TryGetComponent<FruitItem>(out var fruitItem))
        {
            Debug.LogError("FruitItemPrefab에 FruitItem 스크립트가 연결되지 않았습니다.");
            Destroy(fruitItemObject);
            return;
        }

        fruitItem.UpdateFruit(id, count, fruitData.Image);
        _fruitUIItems[id] = fruitItem;
    }

    public void RemoveFruitUI(FruitsID id)
    {
        if (!_fruitUIItems.ContainsKey(id)) return;

        Destroy(_fruitUIItems[id].gameObject);
        _fruitUIItems.Remove(id);
    }

    public void OnFruitSelected(FruitsID selectedFruitID)
    {
        var fruitData = GM.GetFruitsData(selectedFruitID);
        if (fruitData == null)
        {
            Debug.LogWarning($"ID {selectedFruitID}에 해당하는 과일 데이터를 찾을 수 없습니다.");
            return;
        }

        Debug.Log($"과일 선택됨: {selectedFruitID}");
    }

    public Sprite GetFruitImage(FruitsID id)
    {
        return GM.GetFruitsData(id)?.Image;
    }
}
