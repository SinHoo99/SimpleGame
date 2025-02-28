using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager : MonoBehaviour
{
    private GameManager _gm;
    private GameManager GM => _gm ??= GameManager.Instance; //  ������ �̱��� ����

    [SerializeField] private GameObject fruitDictionaryPrefab; // ���� UI ������
    [SerializeField] private Transform dictionaryContent; // ���� UI �θ� ��ü

    private readonly Dictionary<FruitsID, FruitDictionaryItem> _fruitDictionaryItems = new();

    private void Start()
    {
        Debug.Log("[DictionaryManager] Start() ����");

        if (GM == null || GM.DataManager == null || GM.ScoreUpdater == null)
        {
            Debug.LogError("[DictionaryManager] GameManager, DataManager �Ǵ� ScoreUpdater�� �������� �ʽ��ϴ�.");
            return;
        }

        InitializeDictionary(GM.DataManager.FruitDatas);

        //  ScoreUpdater�� ���� �߰� �̺�Ʈ ���� (UI �ڵ� ������Ʈ)
        GM.ScoreUpdater.OnFruitCollected += UpdateDictionaryUI;
        Debug.Log("[DictionaryManager] ScoreUpdater.OnFruitCollected �̺�Ʈ ���� �Ϸ�");
    }

    private void OnDestroy()
    {
        if (GM != null && GM.ScoreUpdater != null)
        {
            GM.ScoreUpdater.OnFruitCollected -= UpdateDictionaryUI;
            Debug.Log("[DictionaryManager] ScoreUpdater.OnFruitCollected �̺�Ʈ ���� �Ϸ�");
        }
    }

    /// <summary>
    /// ���� UI�� �ʱ�ȭ�մϴ�.
    /// </summary>
    public void InitializeDictionary(Dictionary<FruitsID, FruitsData> fruitData)
    {
        Debug.Log("[DictionaryManager] InitializeDictionary() ����");

        if (fruitData == null || fruitData.Count == 0)
        {
            Debug.LogWarning("[DictionaryManager] �ʱ�ȭ�� ���� �����Ͱ� �����ϴ�.");
            return;
        }

        Debug.Log($"[DictionaryManager] {fruitData.Count}���� ���� ������ �ε� �Ϸ�");

        ClearExistingUI();

        //  ���� UI ���� ����
        foreach (var (id, data) in fruitData)
        {
            CreateFruitDictionaryItem(id, data);
        }

        Debug.Log("[DictionaryManager] ��� ���� �׸� ���� �Ϸ�");
        UpdateAllDictionaryUI();
    }

    /// <summary>
    /// ���� UI ��ҵ��� �����մϴ�.
    /// </summary>
    private void ClearExistingUI()
    {
        foreach (var item in _fruitDictionaryItems.Values)
        {
            if (item != null)
                Destroy(item.gameObject);
        }
        _fruitDictionaryItems.Clear();

        foreach (Transform child in dictionaryContent)
        {
            Destroy(child.gameObject);
        }
    }

    /// <summary>
    /// ���ο� ���� UI �׸��� �����մϴ�.
    /// </summary>
    private void CreateFruitDictionaryItem(FruitsID id, FruitsData data)
    {
        if (fruitDictionaryPrefab == null)
        {
            Debug.LogError("[DictionaryManager] fruitDictionaryPrefab�� �������� �ʾҽ��ϴ�.");
            return;
        }

        var itemObj = Instantiate(fruitDictionaryPrefab, dictionaryContent);
        if (!itemObj.TryGetComponent<FruitDictionaryItem>(out var fruitItem))
        {
            Debug.LogError($"[DictionaryManager] {id}�� ���� FruitDictionaryItem ��ũ��Ʈ�� �����ϴ�!");
            Destroy(itemObj);
            return;
        }

        fruitItem.Setup(id, data.Image);
        _fruitDictionaryItems[id] = fruitItem;
        Debug.Log($"[DictionaryManager] ���� �׸� �߰�: {id}");
    }

    /// <summary>
    /// Ư�� ������ UI�� ������Ʈ�մϴ�.
    /// </summary>
    public void UpdateDictionaryUI(FruitsID fruitID)
    {
        if (!_fruitDictionaryItems.TryGetValue(fruitID, out var item))
        {
            Debug.LogWarning($"[DictionaryManager] ������ {fruitID} �������� �����ϴ�.");
            return;
        }

        bool isCollected = GM.PlayerDataManager.IsFruitCollected(fruitID);
        Debug.Log($"[DictionaryManager] {fruitID} UI ������Ʈ - ���� ����: {isCollected}");

        item.SetCollected(isCollected);
    }

    /// <summary>
    /// ��ü ���� UI�� ������Ʈ�մϴ�.
    /// </summary>
    public void UpdateAllDictionaryUI()
    {
        Debug.Log("[DictionaryManager] UpdateAllDictionaryUI() ����");

        foreach (var (id, item) in _fruitDictionaryItems)
        {
            bool isCollected = GM.PlayerDataManager.IsFruitCollected(id);
            Debug.Log($"[DictionaryManager] {id} UI ������Ʈ - ���� ����: {isCollected}");

            item.SetCollected(isCollected);
        }
    }
}
