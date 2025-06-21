using UnityEngine;

public interface IPlayerSoundManager
{
    public void PlaySound(PlayerSoundManager.SoundType soundType, float volume = 1f);
    public void OnDeath();
}

public class PlayerSoundManager : MonoBehaviour, IPlayerSoundManager
{
    [SerializeField] private SoundData soundData;

    private PlayerState playerState;
    private PlayerContext playerContext;

    public void Initialize(PlayerState playerState, PlayerContext playerContext)
    {
        this.playerState = playerState;
        this.playerContext = playerContext;
    }

    public void OnDeath()
    {
        AudioManager.SetBackGroundVolume(0, 1);
    }

    public void PlaySound(SoundType soundType, float volume = 1f)
    {
        switch(soundType)
        {
            case SoundType.Slash:
                AudioManager.Play(soundData.SwordSlash, volume);
                break;
            case SoundType.Parring:
                AudioManager.Play(soundData.Parring, volume);
                break;
            case SoundType.FootStep:
                AudioManager.Play(soundData.FootStep, volume);
                break;
            case SoundType.Dash:
                AudioManager.Play(soundData.Dash, volume);
                break;
            case SoundType.Hit:
                AudioManager.Play(soundData.PlayerHit, volume);
                break;
            default:
                Debug.LogWarning("Invalid Sound Type: " + soundData.name);
                break;
        }
    }

    public enum SoundType
    {
        Slash,
        Parring,
        FootStep,
        Dash,
        Hit
    }
}
