using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LoadView loadViewPrefab;
    public static GameManager Instance { get; private set; }
    public static IPlayerController PlayerController { get; set; }
    public static bool IsGameInitializing { get; private set; }

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        GameObject newGameObject = new GameObject(typeof(GameManager).Name);
        Instance = newGameObject.AddComponent<GameManager>();

        IsGameInitializing = false;
        
        DontDestroyOnLoad(newGameObject);
    }

    public static void Victory(float time)
    {
        if (IsGameInitializing) return;

        PlayerController.StopPlayer(time);
        Instance.LoadScene(SceneType.Start, time/2, time/2);
        Instance.StartCoroutine(InitializeGame(time));
    }

    public static void Defeat(float time)
    {
        if (IsGameInitializing) return;

        PlayerController.StopPlayer(time);
        Instance.LoadScene(SceneType.Start, time/2, time/2);
        Instance.StartCoroutine(InitializeGame(time));
    }

    private static IEnumerator InitializeGame(float time)
    {
        IsGameInitializing = true;
        yield return new WaitForSecondsRealtime(time);
        IsGameInitializing = false;
    }

    public void LoadScene(SceneType sceneType, float fadeTime = 0.5f, float delayTime = 0f)
    {
        if(AudioManager.IsAllocated) AudioManager.SetBackGroundVolume(0, 1);

        switch(sceneType)
        {
            case SceneType.Game:
                StartCoroutine(LoadScene("GameScene", fadeTime, delayTime));
                break;
            case SceneType.Start:
                StartCoroutine(LoadScene("StartScene", fadeTime, delayTime));
                break;
            case SceneType.Tutorial:
                StartCoroutine(LoadScene("TutorialScene", fadeTime, delayTime));
                break;
            default:
                Debug.LogWarning("Invalid Scene Type: " + sceneType.ToString());
                break;
        }
    }

    private IEnumerator LoadScene(string sceneName, float fadeTime, float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);

        LoadView loadView = Instantiate(loadViewPrefab);
        loadView.LoadImage.DOColor(Color.black, fadeTime);
        
        yield return new WaitForSecondsRealtime(fadeTime);
        
        SceneManager.LoadScene(sceneName);

        Vector3 dampingRate = CinemachineManager.Instance.PositionDamping;
        CinemachineManager.Instance.PositionDamping = Vector3.zero;

        loadView.LoadImage
            .DOColor(new Color(0, 0, 0, 0), 0.25f)
            .OnComplete(() => {
                    Destroy(loadView.gameObject);
                    CinemachineManager.Instance.PositionDamping = dampingRate;
                }
            );
    }

    public enum SceneType
    { Start, Game, Tutorial }
}
