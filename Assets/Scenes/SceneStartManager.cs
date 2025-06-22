using UnityEngine;

public class SceneStartManager : MonoBehaviour
{
    [SerializeField] private Vector2 startPosition;
    private void Start()
    {
        PlayerController.Transform.position = startPosition;
    }
}
