using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();
    private List<GameObject> activeObjects = new List<GameObject>();

    public void AddObjectPool(string key, GameObject prefab, int initialSize)
    {
        if (poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Object Pool�� �̹� �����ϴ� Ű�Դϴ�: {key}");
            return;
        }

        Queue<GameObject> objectQueue = new Queue<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            objectQueue.Enqueue(obj);
        }

        poolDictionary.Add(key, objectQueue);
    }

    public GameObject GetObject(string key)
    {
        if (!poolDictionary.ContainsKey(key) || poolDictionary[key].Count == 0)
        {
            Debug.LogWarning($"Object Pool�� {key} Ű�� ���ų� ��� ������ ������Ʈ�� �����ϴ�.");
            return null;
        }

        GameObject obj = poolDictionary[key].Dequeue();
        activeObjects.Add(obj);

        // PoolObject Ȱ��ȭ ó��
        PoolObject poolObject = obj.GetComponent<PoolObject>();
        poolObject?.OnActivatedFromPool();

        return obj;
    }

    public void ReturnObject(string key, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Object Pool�� {key} Ű�� �����ϴ�.");
            return;
        }

        // PoolObject ��ȯ ó��
        PoolObject poolObject = obj.GetComponent<PoolObject>();
        poolObject?.OnReturnedToPool();

        obj.SetActive(false);
        poolDictionary[key].Enqueue(obj);
        activeObjects.Remove(obj);
    }

    public void ReturnAllObjects()
    {
        foreach (var obj in activeObjects.ToArray())
        {
            string key = obj.name.Replace("(Clone)", "").Trim();
            ReturnObject(key, obj);
        }

        activeObjects.Clear();
    }
}
