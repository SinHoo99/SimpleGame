using UnityEngine;

public class PoolObject : MonoBehaviour
{
    public T ReturnMyComponent<T>() where T : PoolObject
    {
        T component = this as T;

        if (component == null)
        {
            Debug.LogWarning($"오브젝트에 {typeof(T).Name} 컴포넌트가 없습니다.");
            return null;
        }
        return component;
    }
}
