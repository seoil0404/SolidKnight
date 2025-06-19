using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static IPlayerController PlayerController { get; set; }

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        GameObject newGameObject = new GameObject(typeof(GameManager).Name);
        Instance = newGameObject.AddComponent<GameManager>();
        DontDestroyOnLoad(newGameObject);
    }

    public static void Victory()
    {
        PlayerController.StopPlayer();
    }

    public static void Defeat()
    {

    }
}
