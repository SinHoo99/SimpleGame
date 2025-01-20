using UnityEngine;

public class PoolObject : MonoBehaviour
{
    /// <summary>
    /// 오브젝트가 Object Pool로 반환될 때 호출
    /// </summary>
    public virtual void OnReturnedToPool()
    {
        // 기본적으로 오브젝트를 비활성화
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 오브젝트가 Object Pool에서 활성화될 때 호출
    /// </summary>
    public virtual void OnActivatedFromPool()
    {
        // 기본적으로 오브젝트를 활성화
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 특정 컴포넌트를 반환
    /// </summary>
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
