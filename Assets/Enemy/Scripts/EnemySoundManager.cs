using UnityEngine;

public interface IEnemySoundManager
{
    public void PlaySound(EnemySoundManager.SoundType soundType, float volume = 1f);
    public void OnDeath();
}

public class EnemySoundManager : MonoBehaviour, IEnemySoundManager
{
    [SerializeField] private SoundData soundData;

    private EnemyState enemyState;
    private EnemyContext enemyContext;

    public void Initialize(EnemyState enemyState, EnemyContext enemyContext)
    {
        this.enemyState = enemyState;
        this.enemyContext = enemyContext;
    }

    public void OnDeath()
    {
        AudioManager.Play(soundData.Clear, 1);
    }

    public void PlaySound(SoundType soundType, float volume = 1f)
    {
        switch(soundType)
        {
            case SoundType.Hit:
                AudioManager.Play(soundData.EnemyHit, volume);
                break;
            case SoundType.GroundHit:
                AudioManager.Play(soundData.GroundHit, volume);
                break;
            default:
                Debug.LogWarning("Invalid SoundType: " + soundType.ToString());
                break;
        }
    }

    public enum SoundType
    {
        Hit,
        GroundHit
    }
}
