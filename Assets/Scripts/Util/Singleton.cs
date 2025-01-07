using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                SetupInstance();
            }

            return _instance;
        }
    }

    [SerializeField] protected bool isDontDestroy = false;

    private static void SetupInstance()
    {
        _instance = (T)FindAnyObjectByType(typeof(T));

        if (_instance == null)
        {
            GameObject gameObject = new GameObject(typeof(T).Name);
            _instance = gameObject.AddComponent<T>();
        }
    }

    protected virtual void Awake()
    {
        if (isDontDestroy)
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    protected bool IsDuplicates()
    {
        if (_instance == null)
        {
            _instance = this as T;

            return false;
        }
        else
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return true;
            }
            return false;
        }
    }
}