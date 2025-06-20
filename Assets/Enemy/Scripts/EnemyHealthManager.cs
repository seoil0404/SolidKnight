using System.Collections;
using UnityEngine;

public interface IEnemyHealthManager
{
    public void ReduceHealth(uint health);
}

public class EnemyHealthManager : MonoBehaviour, IEnemyHealthManager
{
    [SerializeField] private int health;

    private EnemyContext enemyContext;
    private EnemyState enemyState;

    public void Initialize(
        EnemyState enemyState,
        EnemyContext enemyContext
        )
    {
        this.enemyState = enemyState;
        this.enemyContext = enemyContext;
    }

    public void ReduceHealth(uint health)
    {
        this.health -= (int)health;
        if (this.health <= 0) Death();
    }

    private void Death()
    {
        if (GameManager.IsGameInitializing) return;

        float time = 6;
        GameManager.Victory(time);

        enemyContext.MovementHandler.SetVelocity(Vector2.zero);
        enemyContext.CombatHandler.StopAttack();
        enemyContext.RenderManager.OnDeath();
        enemyContext.Controller.StopEnemy();
    }
}
