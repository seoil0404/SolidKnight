using UnityEngine;
using UnityEngine.UI;

public class LoadView : MonoBehaviour
{
    [SerializeField] private Image loadImage;
    public Image LoadImage => loadImage;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
