using UnityEngine;

public class SceneStartManager : MonoBehaviour
{
    [SerializeField] private Vector2 startPosition;
    private void Awake()
    {
        PlayerController.Transform.position = startPosition;
    }
}
