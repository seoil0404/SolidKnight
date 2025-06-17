using UnityEngine;

public class SingletonBehavior : MonoBehaviour
{
    public static SingletonBehavior Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        GameObject newGameObject = new GameObject(typeof(SingletonBehavior).Name);
        Instance = newGameObject.AddComponent<SingletonBehavior>();
        DontDestroyOnLoad(newGameObject);
    }
}
