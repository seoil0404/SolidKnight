using UnityEngine;

public interface IEnemyHealthManager
{
    public void ReduceHealth(uint health);
}

public class EnemyHealthManager : MonoBehaviour, IEnemyHealthManager
{
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
        
    }
}
