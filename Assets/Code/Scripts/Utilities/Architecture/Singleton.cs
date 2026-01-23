using UnityEngine;

/// <summary>
/// Enforces a single instance of the reference type exists in the scene.
/// </summary>
/// <typeparam name="T">Reference type to set as singleton</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_instance;
    public static bool HasInstance { get { return m_instance != null; } }

    public Singleton()
    {
    }

    public static T Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindFirstObjectByType<T>();

                if (m_instance == null)
                    Debug.LogError($"No instance of type {typeof(T)} exists in scene. Please add a {typeof(T)} in the scene.");
            }

            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        if (m_instance == null)
            m_instance = gameObject.GetComponent<T>();

        else if (m_instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
            Debug.LogWarning(string.Format("Instance of {0} already exists, removing {1}", GetType().FullName, ToString()));
        }
    }
}

/// <summary>
/// Enforces a single instance of the reference type exists in the scene. Lazily instantiates an instance if none exist.
/// </summary>
/// <typeparam name="T">Reference type to set as singleton</typeparam>
public abstract class LazySingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T m_instance;
    public static bool HasInstance { get { return m_instance != null; } }

    public LazySingleton()
    {
    }

    public static T Instance
    {
        get
        { 
            if (m_instance == null)
            {
                m_instance = FindFirstObjectByType<T>();

                if (m_instance == null)
                {
                    GameObject go = new GameObject(typeof(T).ToString());
                    m_instance = go.AddComponent<T>();
                }
            }

            return m_instance;
        }
    }

    protected virtual void Awake()
    {
        if (m_instance == null)
            m_instance = gameObject.GetComponent<T>();

        else if (m_instance.GetInstanceID() != GetInstanceID())
        {
            Destroy(gameObject);
            Debug.LogWarning(string.Format("Instance of {0} already exists, removing {1}", GetType().FullName, ToString()));
        }
    }
}