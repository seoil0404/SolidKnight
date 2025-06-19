using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private LoadView loadViewPrefab;
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
        PlayerController.StopPlayer(6);
        Instance.LoadScene(SceneType.Start, 3, 3);
    }

    public static void Defeat()
    {

    }

    public void LoadScene(SceneType sceneType, float fadeTime = 0.5f, float delayTime = 0f)
    {
        switch(sceneType)
        {
            case SceneType.Game:
                StartCoroutine(LoadScene("GameScene", fadeTime, delayTime));
                break;
            case SceneType.Start:
                StartCoroutine(LoadScene("StartScene", fadeTime, delayTime));
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
    { Start, Game }
}
