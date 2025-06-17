using System.Collections;
using UnityEngine;

public interface IPlayerHealthManager
{
    public bool ReduceHealth(uint health);
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

    public bool ReduceHealth(uint health)
    {
        StartCoroutine(StopTime());

        if (playerContext.CombatHandler.IsParring())
        {
            playerContext.CombatHandler.OnParringSucces();
            return false;
        }

        this.health -= (int)health;

        if (health < 0) Death();
        else playerContext.RenderManager.OnGetHit();

        return true;
    }

    private IEnumerator StopTime()
    {
        yield return new WaitForSeconds(0.05f);
        playerContext.RenderManager.SetTimeScale(0);
        yield return new WaitForSecondsRealtime(0.3f);
        playerContext.RenderManager.SetTimeScale(1);
    }

    private void Death()
    {
        playerContext.RenderManager.OnDeath();
    }
}
