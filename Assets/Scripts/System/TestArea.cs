using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArea : MonoBehaviour
{
    [SerializeField] private GameObject[] fruits; 
    [SerializeField] private ObjectPool _objectPool;

    void Start()
    {
        AddObjectPool();
    }

    public void AddObjectPool()
    {
        foreach (var fruit in fruits)
        {
            if (fruit == null)
            {
                Debug.LogWarning("fruits 배열에 null 값이 있습니다.");
                continue;
            }

            // 프리팹 이름을 키로 사용하여 Object Pool에 추가
            _objectPool.AddObjectPool(fruit.name, fruit, 20);
            Debug.Log($"{fruit.name}이(가) Object Pool에 추가되었습니다.");
        }
    }
}
