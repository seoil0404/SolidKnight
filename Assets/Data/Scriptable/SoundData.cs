using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    [SerializeField] private AudioClip swordSlash;
    [SerializeField] private AudioClip parring;
    [SerializeField] private AudioClip footStep;
    [SerializeField] private AudioClip dash;
    [SerializeField] private AudioClip playerHit;

    public AudioClip SwordSlash => swordSlash;
    public AudioClip Parring => parring;
    public AudioClip FootStep => footStep;
    public AudioClip Dash => dash;
    public AudioClip PlayerHit => playerHit;
}
