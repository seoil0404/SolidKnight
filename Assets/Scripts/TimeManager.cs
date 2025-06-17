using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        GameObject newGameObject = new GameObject(typeof(TimeManager).Name);
        Instance = newGameObject.AddComponent<TimeManager>();
        DontDestroyOnLoad(newGameObject);
    }

    public void SetTimeScale(float scale) =>
        Time.timeScale = scale;
}
