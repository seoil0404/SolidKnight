using UnityEngine;

public class SceneMover : MonoBehaviour
{
    [SerializeField] private GameManager.SceneType sceneType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerController _))
        {
            GameManager.Instance.LoadScene(sceneType);
        }
    }
}
