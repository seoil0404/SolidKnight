using UnityEngine;

[CreateAssetMenu(fileName = "SoundData", menuName = "Scriptable Objects/SoundData")]
public class SoundData : ScriptableObject
{
    [SerializeField] private AudioClip swordSlash;
    [SerializeField] private AudioClip parring;
    [SerializeField] private AudioClip footStep;
    [SerializeField] private AudioClip dash;
    [SerializeField] private AudioClip playerHit;
    [SerializeField] private AudioClip enemyHit;
    [SerializeField] private AudioClip clear;
    [SerializeField] private AudioClip groundHit;

    public AudioClip SwordSlash => swordSlash;
    public AudioClip Parring => parring;
    public AudioClip FootStep => footStep;
    public AudioClip Dash => dash;
    public AudioClip PlayerHit => playerHit;
    public AudioClip EnemyHit => enemyHit;
    public AudioClip Clear => clear;
    public AudioClip GroundHit => groundHit;
}
