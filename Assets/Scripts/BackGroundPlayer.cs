using System.Collections;
using UnityEngine;

public class BackGroundPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip backGroundClip;
    [SerializeField] private float volume;

    private void Awake()
    {
        StartCoroutine(SetBackGroundMusic());
    }

    private IEnumerator SetBackGroundMusic()
    {
        yield return new WaitWhile(() => !AudioManager.IsAllocated);
        AudioManager.SetBackgroundMusic(backGroundClip, volume, 1f);
    }
}
