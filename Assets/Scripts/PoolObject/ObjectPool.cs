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
            Debug.LogWarning($"Object Pool에 이미 존재하는 키입니다: {tag}");
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
            Debug.LogWarning($"Pool에 {tag}에 해당하는 오브젝트가 없습니다.");
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

        PoolObject newObj = Instantiate(list[0], transform);
        newObj.gameObject.SetActive(true);
        list.Add(newObj);
        return newObj;
    }

    public PoolObject FindActiveObject(string tag)
    {
        if (!poolDictionary.TryGetValue(tag, out List<PoolObject> list))
        {
            Debug.LogWarning($"Pool에 {tag}에 해당하는 오브젝트가 없습니다.");
            return null;
        }

        foreach (PoolObject obj in list)
        {
            if (obj.gameObject.activeInHierarchy) 
            {
                return obj;
            }
        }

        return null; 
    }

    public void ReturnObject(string tag, PoolObject obj)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning($"Object Pool에 {tag} 키가 없습니다.");
            return;
        }
        InitVector(obj);
        obj.gameObject.SetActive(false);
    }

    public void ReturnAllObjects()
    {
        foreach (var list in poolDictionary.Values)
        {
            foreach (var obj in list)
            {
                obj.gameObject.SetActive(false);
                InitVector(obj);
            }
        }
        Debug.Log("모든 오브젝트가 반환되었습니다.");
    }
    private void InitVector(PoolObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
    }
}
