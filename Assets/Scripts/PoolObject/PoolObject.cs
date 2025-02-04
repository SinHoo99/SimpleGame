using UnityEngine;

public class PoolObject : MonoBehaviour
{
    /// <summary>
    /// Ư�� ������Ʈ�� ��ȯ
    /// </summary>
    public T ReturnMyComponent<T>() where T : PoolObject
    {
        T component = this as T;

        if (component == null)
        {
            Debug.LogWarning($"������Ʈ�� {typeof(T).Name} ������Ʈ�� �����ϴ�.");
            return null;
        }

        return component;
    }
}
