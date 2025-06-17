using UnityEngine;

public interface IPlayerHealthManager
{
    public void ReduceHealth(uint health);
}

public class PlayerHealthManager : MonoBehaviour, IPlayerHealthManager
{
    [SerializeField] private int health;

    private PlayerState playerState;
    private PlayerContext playerContext;

    public void Initialize(
        PlayerState playerState, 
        PlayerContext playerContext
        )
    {
        this.playerState = playerState;
        this.playerContext = playerContext;
    }

    public void ReduceHealth(uint health)
    {
        this.health -= (int)health;

        if (health < 0) Death();
        else playerContext.RenderManager.OnGetHit();
    }

    private void Death()
    {
        playerContext.RenderManager.OnDeath();
    }
}
