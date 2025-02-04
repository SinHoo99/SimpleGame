using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    [SerializeField] private PoolObject[] fruitPrefabs; 

    #region 오브젝트풀 초기화
    public void InitializeObjectPool()
    {
        if (GM.ObjectPool == null)
        {
            Debug.LogError("ObjectPool이 초기화되지 않았습니다.");
            return;
        }

        foreach (var prefab in fruitPrefabs)
        {
            if (prefab == null)
            {
                Debug.LogWarning("fruitPrefabs 배열에 null 값이 있습니다.");
                continue;
            }

            string key = prefab.name.Replace("(Clone)", "").Trim();
            GM.ObjectPool.AddObjectPool(key, prefab, 20);
            Debug.Log($"{key}이(가) Object Pool에 추가되었습니다.");
        }
    }
    #endregion
}
