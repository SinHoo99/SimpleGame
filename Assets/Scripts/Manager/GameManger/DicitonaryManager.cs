using System.Collections.Generic;
using UnityEngine;

public class DictionaryManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;

    [SerializeField] private GameObject fruitDictionaryPrefab; // ���� UI ������
    [SerializeField] private Transform dictionaryContent; // ������ ��ġ�� �θ� ������Ʈ

    private Dictionary<FruitsID, FruitDictionaryItem> _fruitDictionaryItems = new();

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
        //  �̺�Ʈ ���� (�޸� ���� ����)
        if (GM.ScoreUpdater != null)
        {
            GM.ScoreUpdater.OnFruitCollected -= UpdateDictionaryUI;
            Debug.Log("[DictionaryManager] ScoreUpdater.OnFruitCollected �̺�Ʈ ���� �Ϸ�");
        }
    }

    public void InitializeDictionary(Dictionary<FruitsID, FruitsData> fruitData)
    {
        Debug.Log("[DictionaryManager] InitializeDictionary() ����");

        if (fruitData == null || fruitData.Count == 0)
        {
            Debug.LogWarning("[DictionaryManager] �ʱ�ȭ�� ���� �����Ͱ� �����ϴ�.");
            return;
        }

        Debug.Log($"[DictionaryManager] {fruitData.Count}���� ���� ������ �ε� �Ϸ�");

        // ���� UI ������Ʈ ����
        foreach (var item in _fruitDictionaryItems.Values)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }
        _fruitDictionaryItems.Clear();

        foreach (Transform child in dictionaryContent)
        {
            Destroy(child.gameObject);
        }

        // ���ο� ���� UI ���� ����
        foreach (var (id, data) in fruitData)
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
                continue;
            }

            fruitItem.Setup(id, data.Image);
            _fruitDictionaryItems[id] = fruitItem;
            Debug.Log($"[DictionaryManager] ���� �׸� �߰�: {id}");
        }

        Debug.Log("[DictionaryManager] ��� ���� �׸� ���� �Ϸ�");
        UpdateAllDictionaryUI(); // ��� UI ������Ʈ
    }

    //  �̺�Ʈ�� ���� �ڵ����� ����Ǵ� UI ������Ʈ �Լ�
    public void UpdateDictionaryUI(FruitsID fruitID)
    {
        if (!_fruitDictionaryItems.TryGetValue(fruitID, out var item))
        {
            Debug.LogWarning($"[DictionaryManager] ������ {fruitID} �������� �����ϴ�.");
            return;
        }

        //  ������ �ֽ� �����͸� ������ UI ������Ʈ
        bool isCollected = GM.PlayerDataManager.IsFruitCollected(fruitID);
        Debug.Log($"[DictionaryManager] {fruitID} ���� ���� (������Ʈ ����): {isCollected}");

        item.SetCollected(isCollected);
    }


    //  ��� UI�� �ʱ�ȭ�ϴ� �Լ�
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
