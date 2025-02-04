using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Dictionary<string, List<PoolObject>> poolDictionary;

    public Dictionary<string, List<PoolObject>> PoolDictionary => poolDictionary;

    private void Awake()
    {
        poolDictionary = new Dictionary<string, List<PoolObject>>();
    }

    public void AddObjectPool(string tag, PoolObject prefab, int size)
    {
        if (poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Object Pool�� �̹� �����ϴ� Ű�Դϴ�: {tag}");
            return;
        }

        List<PoolObject> objectPool = new List<PoolObject>();

        for (int i = 0; i < size; i++)
        {
            PoolObject obj = Instantiate(prefab, transform);
            obj.gameObject.SetActive(false);
            objectPool.Add(obj);
        }

        poolDictionary.Add(tag, objectPool);
    }

    public PoolObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.TryGetValue(tag, out List<PoolObject> list))
        {
            Debug.LogWarning($"Pool�� {tag}�� �ش��ϴ� ������Ʈ�� �����ϴ�.");
            return null;
        }

        foreach (PoolObject obj in list)
        {
            if (!obj.gameObject.activeInHierarchy)
            {
                obj.gameObject.SetActive(true);
                return obj;
            }
        }

        Debug.LogWarning($"{tag} Ǯ���� ����� �� �ִ� ������Ʈ�� �����ϴ�.");
        return null;
    }

    public void ReturnObject(string tag, PoolObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Object Pool�� {tag} Ű�� �����ϴ�.");
            return;
        }

        obj.gameObject.SetActive(false);
    }

    public void ReturnAllObjects()
    {
        foreach (var list in poolDictionary.Values)
        {
            foreach (var obj in list)
            {
                obj.gameObject.SetActive(false);
            }
        }

        Debug.Log("��� ������Ʈ�� ��ȯ�Ǿ����ϴ�.");
    }
}
