using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [Header("UI Components")]
    public GameObject fruitItemPrefab; // ���� UI ������
    public Transform fruitListParent; // ���� UI �θ� ��ü

    private readonly Dictionary<FruitsID, FruitItem> _fruitUIItems = new(); // ������ UI�� ȿ�������� �����Ͽ� �ߺ� ���� ����, ���� ������Ʈ/������ ���� ���
    private Dictionary<FruitsID, FruitsData> _fruitData; // �����Ϳ� UI�� �и��ϰ�, �ʿ��� �����͸� ������ �����ϱ� ���� ���.
    public event Action OnInventoryUpdated;

    private void OnEnable()
    {
        OnInventoryUpdated += HandleInventoryUI;
    }

    private void OnDestroy()
    {
        OnInventoryUpdated -= HandleInventoryUI;
    }

    private void HandleInventoryUI()
    {
        UpdateUIWithInventory();
    }

    public void TriggerInventoryUpdate()
    {
        OnInventoryUpdated?.Invoke();
        Debug.Log("TriggerInventoryUpdate ����");
    }

    #region ���� ������ UI ���� ����
    /// ���� �����͸� �����մϴ�.
    public void SetFruitData(Dictionary<FruitsID, FruitsData> fruitData)
    {
        _fruitData = fruitData ?? new Dictionary<FruitsID, FruitsData>();
    }

    /// ���� ������ ���� UI�� ������Ʈ�մϴ�.
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

    /// ���� UI�� �����ϰų� ������Ʈ�մϴ�.
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

    /// ���� UI �׸��� �����մϴ�.
    private void CreateFruitUI(FruitsID id, int count)
    {
        if (!TryGetFruitData(id, out var fruitData))
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

    /// ���� ���� UI �׸��� ������Ʈ�մϴ�.
    private void UpdateFruitUI(FruitItem fruitItem, FruitsID id, int count)
    {
        if (TryGetFruitData(id, out var fruitData))
        {
            fruitItem.UpdateFruit(id, count, fruitData.Image);
        }
    }

    /// ���� UI �׸��� �����մϴ�.
    private void RemoveFruitUI(FruitsID id)
    {
        if (_fruitUIItems.TryGetValue(id, out var fruitItem))
        {
            Destroy(fruitItem.gameObject);
            _fruitUIItems.Remove(id);
        }
    }

    /// UI �ʱ�ȭ ���¸� Ȯ���մϴ�.
    private bool IsUIInitialized()
    {
        if (fruitItemPrefab == null || fruitListParent == null || _fruitData == null)
        {
            Debug.LogWarning("UIManager�� �ùٸ��� �ʱ�ȭ���� �ʾҽ��ϴ�!");
            return false;
        }
        return true;
    }

    /// ���� ���� ó��
    public void OnFruitSelected(FruitsID selectedFruitID)
    {
        if (!_fruitData.TryGetValue(selectedFruitID, out var fruitData))
        {
            Debug.LogWarning($"ID {selectedFruitID}�� �ش��ϴ� ���� �����͸� ã�� �� �����ϴ�.");
            return;
        }
    }
    public void ClearAllFruitUI()
    {
        foreach (var fruitItem in _fruitUIItems.Values)
        {
            Destroy(fruitItem.gameObject); // UI ������Ʈ ����
        }

        _fruitUIItems.Clear(); // ���� ������ �ʱ�ȭ
        Debug.Log("��� ���� UI�� ���ŵǾ����ϴ�.");
    }
    #endregion

    #region  UI�� ������Ʈ�ϴ� ���� �޼���
    public void UpdateUIWithInventory()
    {
        GM.PlayerStatusUI.PlayerCoin();

        if (GM.PlayerDataManager.NowPlayerData.Inventory == null || GM.PlayerDataManager.NowPlayerData.Inventory.Count == 0)
        {
            // Inventory�� ����� �� UI�� �ʱ�ȭ
            ClearAllFruitUI();
            Debug.Log("Inventory�� ��� �־� UI�� �ʱ�ȭ�߽��ϴ�.");
            return;
        }
        // Inventory�� �ִ� ���� �����͸� UI�� ������Ʈ
        UpdateFruitCountsUI(
            GM.PlayerDataManager.NowPlayerData.Inventory.ToDictionary(kv => kv.Key, kv => kv.Value.Amount)
         );
    }
    #endregion

    #region  ���� �����͸� �������� bool ��
    private bool TryGetFruitData(FruitsID id, out FruitsData fruitsData)
    {
        if (!_fruitData.TryGetValue(id, out fruitsData))
        {
            return false;
        }
        return true;
    }

    #endregion
}
