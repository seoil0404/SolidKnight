using UnityEngine;

public class SceneMover : MonoBehaviour
{
    [SerializeField] private SceneMoveView sceneMoveViewPrefab;
    [SerializeField] private GameManager.SceneType sceneType;

    private SceneMoveView currentView;
    private bool isEnter = false;
    private bool isUsed = false;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController _) && !isUsed)
        {
            currentView = Instantiate(sceneMoveViewPrefab, transform);
            isEnter = true;
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyData.Enter) && isEnter)
        {
            GameManager.Instance.LoadScene(sceneType);
            currentView.Disappear();
            isUsed = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerController _))
        {
            if(currentView != null)
                currentView.Disappear();

            isEnter = false;
        }
    }
}
