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
    /// ���� ������ ���� (GameManager���� ����)
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
            Debug.LogWarning($"���� ������({id})�� ã�� �� �����ϴ�.");
            return;
        }

        var fruitItemObject = Instantiate(fruitItemPrefab, fruitListParent);
        var fruitItem = fruitItemObject.GetComponent<FruitItem>();

        if (fruitItem == null)
        {
            Debug.LogError("FruitItemPrefab�� FruitItem ��ũ��Ʈ�� ������� �ʾҽ��ϴ�.");
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
            Debug.LogWarning($"���� UI({id})�� ã�� �� �����ϴ�.");
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
            Debug.LogWarning("UIManager�� �ùٸ��� �ʱ�ȭ���� �ʾҽ��ϴ�!");
            return false;
        }
        return true;
    }

    public void OnFruitSelected(FriutsID selectedFruitID)
    {
        if (!_fruitData.TryGetValue(selectedFruitID, out var fruitData))
        {
            Debug.LogWarning($"ID {selectedFruitID}�� �ش��ϴ� ���� �����͸� ã�� �� �����ϴ�.");
            return;
        }

        if (!GameManager.Instance.NowPlayerData.Inventory.TryGetValue(selectedFruitID, out var playerFruitData))
        {
            Debug.LogWarning($"ID {selectedFruitID}�� �÷��̾��� �κ��丮�� �����ϴ�.");
            return;
        }

        Debug.Log($"���� �̸�: {fruitData.Name}");
        Debug.Log($"���� ����: {playerFruitData.Amount}");
        Debug.Log($"����: {fruitData.Description}");
        Debug.Log($"����: {fruitData.Price}");
    }
}
