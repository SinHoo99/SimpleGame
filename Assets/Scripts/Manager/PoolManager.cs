using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private GameManager GM => GameManager.Instance;
    [SerializeField] private PoolObject[] fruitPrefabs; 

    #region ������ƮǮ �ʱ�ȭ
    public void InitializeObjectPool()
    {
        if (GM.ObjectPool == null)
        {
            Debug.LogError("ObjectPool�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        foreach (var prefab in fruitPrefabs)
        {
            if (prefab == null)
            {
                Debug.LogWarning("fruitPrefabs �迭�� null ���� �ֽ��ϴ�.");
                continue;
            }

            string key = prefab.name.Replace("(Clone)", "").Trim();
            GM.ObjectPool.AddObjectPool(key, prefab, 20);
            Debug.Log($"{key}��(��) Object Pool�� �߰��Ǿ����ϴ�.");
        }
    }
    #endregion
}
