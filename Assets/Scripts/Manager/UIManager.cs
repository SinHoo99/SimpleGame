using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject fruitItemPrefab; // 과일 UI 프리팹
    public Transform fruitListParent; // 과일 UI 부모 객체

    private Dictionary<FruitsID, FruitItem> _fruitUIItems = new();
    private Dictionary<FruitsID, FruitsData> _fruitData;

    /// <summary>
    /// 과일 데이터를 설정합니다.
    /// </summary>
    public void SetFruitData(Dictionary<FruitsID, FruitsData> fruitData)
    {
        _fruitData = fruitData ?? new Dictionary<FruitsID, FruitsData>();
    }

    /// <summary>
    /// 과일 수량에 따라 UI를 업데이트합니다.
    /// </summary>
    public void UpdateFruitCountsUI(Dictionary<FruitsID, int> fruitCounts)
    {
        if (!IsUIInitialized()) return;

        foreach (var (fruitID, count) in fruitCounts)
        {
            if (count > 0)
            {
                UpdateOrCreateFruitUI(fruitID, count);
            }
            else
            {
                RemoveFruitUI(fruitID);
            }
        }
    }

    /// <summary>
    /// 과일 UI를 생성하거나 업데이트합니다.
    /// </summary>
    public void UpdateOrCreateFruitUI(FruitsID id, int count)
    {
        if (_fruitUIItems.TryGetValue(id, out var fruitItem))
        {
            UpdateFruitUI(fruitItem, id, count);
        }
        else
        {
            CreateFruitUI(id, count);
        }
    }

    /// <summary>
    /// 과일 UI 항목을 생성합니다.
    /// </summary>
    private void CreateFruitUI(FruitsID id, int count)
    {
        if (!_fruitData.TryGetValue(id, out var fruitData))
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

    /// <summary>
    /// 기존 과일 UI 항목을 업데이트합니다.
    /// </summary>
    private void UpdateFruitUI(FruitItem fruitItem, FruitsID id, int count)
    {
        if (_fruitData.TryGetValue(id, out var fruitData))
        {
            fruitItem.UpdateFruit(id, count, fruitData.Image);
        }
    }

    /// <summary>
    /// 과일 UI 항목을 제거합니다.
    /// </summary>
    private void RemoveFruitUI(FruitsID id)
    {
        if (_fruitUIItems.TryGetValue(id, out var fruitItem))
        {
            Destroy(fruitItem.gameObject);
            _fruitUIItems.Remove(id);
        }
    }

    /// <summary>
    /// UI 초기화 상태를 확인합니다.
    /// </summary>
    private bool IsUIInitialized()
    {
        if (fruitItemPrefab == null || fruitListParent == null || _fruitData == null)
        {
            Debug.LogWarning("UIManager가 올바르게 초기화되지 않았습니다!");
            return false;
        }
        return true;
    }

    /// <summary>
    /// 과일 선택 처리
    /// </summary>
    public void OnFruitSelected(FruitsID selectedFruitID)
    {
        if (!_fruitData.TryGetValue(selectedFruitID, out var fruitData))
        {
            Debug.LogWarning($"ID {selectedFruitID}에 해당하는 과일 데이터를 찾을 수 없습니다.");
            return;
        }
    }

    public void ClearAllFruitUI()
    {
        foreach (var fruitItem in _fruitUIItems.Values)
        {
            Destroy(fruitItem.gameObject); // UI 오브젝트 제거
        }

        _fruitUIItems.Clear(); // 내부 데이터 초기화
        Debug.Log("모든 과일 UI가 제거되었습니다.");
    }
}
