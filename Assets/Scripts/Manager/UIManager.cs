using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject fruitItemPrefab; // 동적으로 생성할 프리팹
    public Transform fruitListParent; // 생성된 UI를 배치할 부모 객체

    private Dictionary<FriutsID, FruitItem> _fruitUIItems = new Dictionary<FriutsID, FruitItem>();
    private Dictionary<FriutsID, FriutsData> _fruitData; // 초기화 시 설정되는 불변 데이터

    /// <summary>
    /// 불변 데이터 초기화
    /// </summary>
    public void InitializeFruitData(Dictionary<FriutsID, FriutsData> fruitData)
    {
        _fruitData = fruitData;
    }

    /// <summary>
    /// 과일 개수를 기반으로 UI를 업데이트합니다.
    /// </summary>
    public void UpdateFruitCountsUI(Dictionary<FriutsID, int> fruitCounts)
    {
        if (fruitItemPrefab == null || fruitListParent == null || _fruitData == null)
        {
            Debug.LogWarning("UIManager가 올바르게 초기화되지 않았습니다!");
            return;
        }

        foreach (var fruit in fruitCounts)
        {
            if (fruit.Value > 0) // 0개 이상인 과일만 표시
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
    /// 새로운 과일 UI를 생성합니다.
    /// </summary>
    private void CreateFruitUI(FriutsID id, int count)
    {
        if (!_fruitData.TryGetValue(id, out FriutsData fruitData))
        {
            Debug.LogWarning($"{id}에 대한 과일 데이터가 없습니다!");
            return;
        }

        GameObject newFruitItemObj = Instantiate(fruitItemPrefab, fruitListParent);
        FruitItem fruitItem = newFruitItemObj.GetComponent<FruitItem>();

        if (fruitItem == null)
        {
            Debug.LogError("FruitItemPrefab에 FruitItem 스크립트가 연결되지 않았습니다!");
            Destroy(newFruitItemObj);
            return;
        }

        fruitItem.UpdateFruit(fruitData.Name, count, fruitData.Image);
        _fruitUIItems[id] = fruitItem;
    }

    /// <summary>
    /// 기존 과일 UI를 업데이트합니다.
    /// </summary>
    private void UpdateFruitItem(FruitItem fruitItem, FriutsID id, int count)
    {
        if (_fruitData.TryGetValue(id, out FriutsData fruitData))
        {
            fruitItem.UpdateFruit(fruitData.Name, count, fruitData.Image);
        }
    }

    /// <summary>
    /// 과일 UI를 제거합니다.
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
