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

    /// ���� UI�� �ʱ�ȭ�մϴ�.
    public void InitializeDictionary(Dictionary<FruitsID, FruitsData> fruitData)
    {
        if (fruitData == null || fruitData.Count == 0) return;

        ClearExistingUI();

        foreach (var (id, data) in fruitData)
        {
            CreateFruitDictionaryItem(id, data);
        }

        UpdateAllDictionaryUI();
    }

    /// ���� UI ��ҵ��� �����մϴ�.
    private void ClearExistingUI()
    {
        foreach (var item in _fruitDictionaryItems.Values)
        {
            if (item != null) Destroy(item.gameObject);
        }
        _fruitDictionaryItems.Clear();

        foreach (Transform child in dictionaryContent)
        {
            Destroy(child.gameObject);
        }
    }

    /// ���ο� ���� UI �׸��� �����մϴ�.
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
    }

    /// Ư�� ������ UI�� ������Ʈ�մϴ�.
    public void UpdateDictionaryUI(FruitsID fruitID)
    {
        if (!_fruitDictionaryItems.TryGetValue(fruitID, out var item))
        {
            Debug.LogWarning($"[DictionaryManager] ������ {fruitID} �������� �����ϴ�.");
            return;
        }

        bool isCollected = GM.GetFruitAmount(fruitID) > 0;
        item.SetCollected(isCollected);
    }

    /// ��ü ���� UI�� ������Ʈ�մϴ�.
    public void UpdateAllDictionaryUI()
    {
        foreach (var (id, item) in _fruitDictionaryItems)
        {
            bool isCollected = GM.GetFruitAmount(id) > 0;
            item.SetCollected(isCollected);
        }
    }
}
