using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject fruitItemPrefab; // �������� ������ ������
    public Transform fruitListParent; // ������ UI�� ��ġ�� �θ� ��ü

    private Dictionary<FriutsID, FruitItem> _fruitUIItems = new Dictionary<FriutsID, FruitItem>();
    private Dictionary<FriutsID, FriutsData> _fruitData; // �ʱ�ȭ �� �����Ǵ� �Һ� ������

    /// <summary>
    /// �Һ� ������ �ʱ�ȭ
    /// </summary>
    public void InitializeFruitData(Dictionary<FriutsID, FriutsData> fruitData)
    {
        _fruitData = fruitData;
    }

    /// <summary>
    /// ���� ������ ������� UI�� ������Ʈ�մϴ�.
    /// </summary>
    public void UpdateFruitCountsUI(Dictionary<FriutsID, int> fruitCounts)
    {
        if (fruitItemPrefab == null || fruitListParent == null || _fruitData == null)
        {
            Debug.LogWarning("UIManager�� �ùٸ��� �ʱ�ȭ���� �ʾҽ��ϴ�!");
            return;
        }

        foreach (var fruit in fruitCounts)
        {
            if (fruit.Value > 0) // 0�� �̻��� ���ϸ� ǥ��
            {
                if (!_fruitUIItems.TryGetValue(fruit.Key, out var fruitItem))
                {
                    CreateFruitUI(fruit.Key, fruit.Value);
                }
                else
                {
                    UpdateFruitItem(fruitItem, fruit.Key, fruit.Value);
                }
            }
            else if (_fruitUIItems.ContainsKey(fruit.Key))
            {
                RemoveFruitUI(fruit.Key);
            }
        }
    }

    /// <summary>
    /// ���ο� ���� UI�� �����մϴ�.
    /// </summary>
    private void CreateFruitUI(FriutsID id, int count)
    {
        if (!_fruitData.TryGetValue(id, out FriutsData fruitData))
        {
            Debug.LogWarning($"{id}�� ���� ���� �����Ͱ� �����ϴ�!");
            return;
        }

        GameObject newFruitItemObj = Instantiate(fruitItemPrefab, fruitListParent);
        FruitItem fruitItem = newFruitItemObj.GetComponent<FruitItem>();

        if (fruitItem == null)
        {
            Debug.LogError("FruitItemPrefab�� FruitItem ��ũ��Ʈ�� ������� �ʾҽ��ϴ�!");
            Destroy(newFruitItemObj);
            return;
        }

        fruitItem.UpdateFruit(fruitData.Name, count, fruitData.Image);
        _fruitUIItems[id] = fruitItem;
    }

    /// <summary>
    /// ���� ���� UI�� ������Ʈ�մϴ�.
    /// </summary>
    private void UpdateFruitItem(FruitItem fruitItem, FriutsID id, int count)
    {
        if (_fruitData.TryGetValue(id, out FriutsData fruitData))
        {
            fruitItem.UpdateFruit(fruitData.Name, count, fruitData.Image);
        }
    }

    /// <summary>
    /// ���� UI�� �����մϴ�.
    /// </summary>
    private void RemoveFruitUI(FriutsID id)
    {
        if (_fruitUIItems.TryGetValue(id, out var fruitItem))
        {
            Destroy(fruitItem.gameObject);
            _fruitUIItems.Remove(id);
        }
    }
}
