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
            Debug.LogWarning($"Object Pool에 이미 존재하는 키입니다: {key}");
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
            Debug.LogWarning($"Object Pool에 {key} 키가 없거나 사용 가능한 오브젝트가 없습니다.");
            return null;
        }

        GameObject obj = poolDictionary[key].Dequeue();
        activeObjects.Add(obj);

        // PoolObject 활성화 처리
        PoolObject poolObject = obj.GetComponent<PoolObject>();
        poolObject?.OnActivatedFromPool();

        return obj;
    }

    public void ReturnObject(string key, GameObject obj)
    {
        if (!poolDictionary.ContainsKey(key))
        {
            Debug.LogWarning($"Object Pool에 {key} 키가 없습니다.");
            return;
        }

        // PoolObject 반환 처리
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
