using UnityEngine;

public class PoolObject : MonoBehaviour
{
    /// <summary>
    /// ������Ʈ�� Object Pool�� ��ȯ�� �� ȣ��
    /// </summary>
    public virtual void OnReturnedToPool()
    {
        // �⺻������ ������Ʈ�� ��Ȱ��ȭ
        gameObject.SetActive(false);
    }

    /// <summary>
    /// ������Ʈ�� Object Pool���� Ȱ��ȭ�� �� ȣ��
    /// </summary>
    public virtual void OnActivatedFromPool()
    {
        // �⺻������ ������Ʈ�� Ȱ��ȭ
        gameObject.SetActive(true);
    }

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
