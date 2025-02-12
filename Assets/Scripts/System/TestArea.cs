using UnityEngine;

public class TestArea : MonoBehaviour
{
    [SerializeField] private PoolObject[] fruits;

    public void AddObjectPool()
    {
        if (GameManager.Instance.objectPool == null)
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

            string cleanKey = fruit.name.Replace("(Clone)", "").Trim();
            GameManager.Instance.objectPool.AddObjectPool(cleanKey, fruit, 20);
            Debug.Log($"{cleanKey}��(��) Object Pool�� �߰��Ǿ����ϴ�.");
        }
    }
}
