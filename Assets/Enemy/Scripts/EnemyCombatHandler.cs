using UnityEngine;

public interface IEnemyCombatHandler
{

}

public class EnemyCombatHandler : MonoBehaviour, IEnemyCombatHandler
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
}
