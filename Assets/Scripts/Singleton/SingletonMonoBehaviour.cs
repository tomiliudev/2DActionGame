using System.Collections;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.Find("SingletonRoot").AddComponent<T>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        Debug.Log("Awake Singleton " + GetType().Name);
        if (instance == null)
        {
            instance = this as T;
        }
        else if (instance != this)
        {
            Debug.Log("I'm one of duplicated singleton instance. sucide.");
            Destroy(this);
            instance = this as T;
        }
    }
}
