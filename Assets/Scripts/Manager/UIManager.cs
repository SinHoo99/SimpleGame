using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [Header("UI Components")]
    public GameObject fruitItemPrefab; // ���� UI ������
    public Transform fruitListParent; // ���� UI �θ� ��ü

    private Dictionary<FruitsID, FruitItem> _fruitUIItems = new(); // ������ UI�� ȿ�������� �����Ͽ� �ߺ� ���� ����, ���� ������Ʈ/������ ���� ���
    private Dictionary<FruitsID, FruitsData> _fruitData; // �����Ϳ� UI�� �и��ϰ�, �ʿ��� �����͸� ������ �����ϱ� ���� ���.

    /// <summary>
    /// ���� �����͸� �����մϴ�.
    /// </summary>
    public void SetFruitData(Dictionary<FruitsID, FruitsData> fruitData)
    {
        _fruitData = fruitData ?? new Dictionary<FruitsID, FruitsData>();
    }

    /// <summary>
    /// ���� ������ ���� UI�� ������Ʈ�մϴ�.
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
    /// ���� UI�� �����ϰų� ������Ʈ�մϴ�.
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
    /// ���� UI �׸��� �����մϴ�.
    /// </summary>
    private void CreateFruitUI(FruitsID id, int count)
    {
        if (!_fruitData.TryGetValue(id, out var fruitData))
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

    /// <summary>
    /// ���� ���� UI �׸��� ������Ʈ�մϴ�.
    /// </summary>
    private void UpdateFruitUI(FruitItem fruitItem, FruitsID id, int count)
    {
        if (_fruitData.TryGetValue(id, out var fruitData))
        {
            fruitItem.UpdateFruit(id, count, fruitData.Image);
        }
    }

    /// <summary>
    /// ���� UI �׸��� �����մϴ�.
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
    /// UI �ʱ�ȭ ���¸� Ȯ���մϴ�.
    /// </summary>
    private bool IsUIInitialized()
    {
        if (fruitItemPrefab == null || fruitListParent == null || _fruitData == null)
        {
            Debug.LogWarning("UIManager�� �ùٸ��� �ʱ�ȭ���� �ʾҽ��ϴ�!");
            return false;
        }
        return true;
    }

    /// <summary>
    /// ���� ���� ó��
    /// </summary>
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

    #region  UI�� ������Ʈ�ϴ� ���� �޼���
    public void UpdateUIWithInventory()
    {
        GM.PlayerStatusUI.PlayerCoin();

        if (GM.PlayerDataManager.NowPlayerData.Inventory == null || GM.PlayerDataManager.NowPlayerData.Inventory.Count == 0)
        {
            // Inventory�� ����� �� UI�� �ʱ�ȭ
            GM.UIManager.ClearAllFruitUI();
            Debug.Log("Inventory�� ��� �־� UI�� �ʱ�ȭ�߽��ϴ�.");
            return;
        }

        // Inventory�� �ִ� ���� �����͸� UI�� ������Ʈ
        GM.UIManager.UpdateFruitCountsUI(
           GM.PlayerDataManager.NowPlayerData.Inventory.ToDictionary(kv => kv.Key, kv => kv.Value.Amount)
        );
    }
    #endregion

}
