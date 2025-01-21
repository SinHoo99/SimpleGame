using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [Header("UI Components")]
    public GameObject fruitItemPrefab; // 과일 UI 프리팹
    public Transform fruitListParent; // 과일 UI 부모 객체

    private Dictionary<FruitsID, FruitItem> _fruitUIItems = new(); // 생성된 UI를 효율적으로 관리하여 중복 생성 방지, 빠른 업데이트/삭제를 위해 사용
    private Dictionary<FruitsID, FruitsData> _fruitData; // 데이터와 UI를 분리하고, 필요한 데이터를 빠르게 참조하기 위해 사용.

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

    #region  UI를 업데이트하는 헬퍼 메서드
    public void UpdateUIWithInventory()
    {
        GM.PlayerStatusUI.PlayerCoin();

        if (GM.PlayerDataManager.NowPlayerData.Inventory == null || GM.PlayerDataManager.NowPlayerData.Inventory.Count == 0)
        {
            // Inventory가 비었을 때 UI를 초기화
            GM.UIManager.ClearAllFruitUI();
            Debug.Log("Inventory가 비어 있어 UI를 초기화했습니다.");
            return;
        }

        // Inventory에 있는 과일 데이터를 UI에 업데이트
        GM.UIManager.UpdateFruitCountsUI(
           GM.PlayerDataManager.NowPlayerData.Inventory.ToDictionary(kv => kv.Key, kv => kv.Value.Amount)
        );
    }
    #endregion

}
