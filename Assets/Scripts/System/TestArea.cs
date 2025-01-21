using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestArea : MonoBehaviour
{
    [SerializeField] private GameObject[] fruits;

    public void AddObjectPool()
    {
        if (GameManager.Instance.ObjectPool == null)
        {
            Debug.LogError("ObjectPool�� �ʱ�ȭ���� �ʾҽ��ϴ�.");
            return;
        }

        foreach (var fruit in fruits)
        {
            if (fruit == null)
            {
                Debug.LogWarning("fruits �迭�� null ���� �ֽ��ϴ�.");
                continue;
            }

            // Ű ���� �� Object Pool�� �߰�
            string cleanKey = fruit.name.Replace("(Clone)", "").Trim();
            GameManager.Instance.ObjectPool.AddObjectPool(cleanKey, fruit, 20);
            Debug.Log($"{cleanKey}��(��) Object Pool�� �߰��Ǿ����ϴ�.");
        }
    }
}
