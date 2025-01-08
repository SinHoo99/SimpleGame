using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject fruitItemPrefab;
    public Transform fruitListParent;

    private Dictionary<FriutsID, FruitItem> _fruitUIItems = new Dictionary<FriutsID, FruitItem>();
    private Dictionary<FriutsID, FriutsData> _fruitData;

    /// <summary>
    /// 과일 데이터 설정 (GameManager에서 전달)
    /// </summary>
    public void SetFruitData(Dictionary<FriutsID, FriutsData> fruitData)
    {
        _fruitData = fruitData;
    }

    public void UpdateFruitCountsUI(Dictionary<FriutsID, int> fruitCounts)
    {
        if (!IsUIInitialized()) return;

        foreach (var fruit in fruitCounts)
        {
            if (fruit.Value > 0)
            {
                if (!_fruitUIItems.ContainsKey(fruit.Key))
                {
                    CreateFruitUI(fruit.Key, fruit.Value);
                }
                else
                {
                    UpdateFruitUI(fruit.Key, fruit.Value);
                }
            }
            else if (_fruitUIItems.ContainsKey(fruit.Key))
            {
                RemoveFruitUI(fruit.Key);
            }
        }
    }

    private void CreateFruitUI(FriutsID id, int count)
    {
        if (!_fruitData.TryGetValue(id, out var fruitData))
        {
            Debug.LogWarning($"과일 데이터({id})를 찾을 수 없습니다.");
            return;
        }

        var fruitItemObject = Instantiate(fruitItemPrefab, fruitListParent);
        var fruitItem = fruitItemObject.GetComponent<FruitItem>();

        if (fruitItem == null)
        {
            Debug.LogError("FruitItemPrefab에 FruitItem 스크립트가 연결되지 않았습니다.");
            Destroy(fruitItemObject);
            return;
        }

        fruitItem.UpdateFruit(id, count, fruitData.Image);
        _fruitUIItems[id] = fruitItem;
    }

    private void UpdateFruitUI(FriutsID id, int count)
    {
        if (!_fruitUIItems.TryGetValue(id, out var fruitItem))
        {
            Debug.LogWarning($"과일 UI({id})를 찾을 수 없습니다.");
            return;
        }

        if (_fruitData.TryGetValue(id, out var fruitData))
        {
            fruitItem.UpdateFruit(id, count, fruitData.Image);
        }
    }

    private void RemoveFruitUI(FriutsID id)
    {
        if (_fruitUIItems.TryGetValue(id, out var fruitItem))
        {
            Destroy(fruitItem.gameObject);
            _fruitUIItems.Remove(id);
        }
    }

    private bool IsUIInitialized()
    {
        if (fruitItemPrefab == null || fruitListParent == null || _fruitData == null)
        {
            Debug.LogWarning("UIManager가 올바르게 초기화되지 않았습니다!");
            return false;
        }
        return true;
    }

    public void OnFruitSelected(FriutsID selectedFruitID)
    {
        if (!_fruitData.TryGetValue(selectedFruitID, out var fruitData))
        {
            Debug.LogWarning($"ID {selectedFruitID}에 해당하는 과일 데이터를 찾을 수 없습니다.");
            return;
        }

        if (!GameManager.Instance.NowPlayerData.Inventory.TryGetValue(selectedFruitID, out var playerFruitData))
        {
            Debug.LogWarning($"ID {selectedFruitID}가 플레이어의 인벤토리에 없습니다.");
            return;
        }

        Debug.Log($"과일 이름: {fruitData.Name}");
        Debug.Log($"현재 개수: {playerFruitData.Amount}");
        Debug.Log($"설명: {fruitData.Description}");
        Debug.Log($"가격: {fruitData.Price}");
    }
}
