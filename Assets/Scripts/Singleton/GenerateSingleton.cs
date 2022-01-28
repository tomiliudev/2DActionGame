using UnityEngine;

public class GenerateSingleton : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static private void Initialize()
    {
        var singletonRootObj = GameObject.Find("SingletonRoot");
        if (singletonRootObj == null)
        {
            singletonRootObj = new GameObject("SingletonRoot");
        }
        DontDestroyOnLoad(singletonRootObj);

        singletonRootObj.AddComponent<SoundManager>();
        //singletonRootObj.AddComponent<TouchManager>();// TODO 保留
    }
}
