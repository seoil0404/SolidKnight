using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class AudioManagerGenerator : MonoBehaviour
{
    [SerializeField] private AudioManager audioManagerPrefab;

    private void Start()
    {
        Instantiate(audioManagerPrefab);
        Destroy(gameObject);
    }

    [RuntimeInitializeOnLoadMethod]
    private static void Initialize()
    {
        //GameObject newObject = new GameObject(typeof(AudioManagerGenerator).Name);
        //newObject.AddComponent<AudioManagerGenerator>();
    }
}
