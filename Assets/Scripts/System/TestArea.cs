using UnityEngine;

public class TestArea : MonoBehaviour
{
    [SerializeField] private PoolObject[] fruits;

    public void AddObjectPool()
    {
        if (GameManager.Instance.objectPool == null)
        {
            Debug.LogError("ObjectPool이 초기화되지 않았습니다.");
            return;
        }

        foreach (var fruit in fruits)
        {
            if (fruit == null)
            {
                Debug.LogWarning("fruits 배열에 null 값이 있습니다.");
                continue;
            }

            string cleanKey = fruit.name.Replace("(Clone)", "").Trim();
            GameManager.Instance.objectPool.AddObjectPool(cleanKey, fruit, 20);
            Debug.Log($"{cleanKey}이(가) Object Pool에 추가되었습니다.");
        }
    }
}
