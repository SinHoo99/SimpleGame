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
                Debug.LogWarning("fruits �迭�� null ���� �ֽ��ϴ�.");
                continue;
            }

            // ������ �̸��� Ű�� ����Ͽ� Object Pool�� �߰�
            _objectPool.AddObjectPool(fruit.name, fruit, 20);
            Debug.Log($"{fruit.name}��(��) Object Pool�� �߰��Ǿ����ϴ�.");
        }
    }
}
