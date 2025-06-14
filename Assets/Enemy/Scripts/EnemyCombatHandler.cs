using System.Collections;
using UnityEngine;

public interface IEnemyCombatHandler
{
    public void OnEndAttack();
}

public class EnemyCombatHandler : MonoBehaviour, IEnemyCombatHandler
{
    [SerializeField] private EnemyPatternData patternData;
    [SerializeField] private float attackInterval;

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

    public void HandleCombat()
    {
        if(enemyState.Behavior == EnemyState.BehaviorType.Idle)
        {
            enemyState.Behavior = EnemyState.BehaviorType.Attack;

            int randomIndex = Random.Range(0, patternData.AttackPatterns.Count);
            patternData.AttackPatterns[randomIndex].StartAttack(enemyState, enemyContext);
        }
    }

    public void OnEndAttack()
    {
        StartCoroutine(EndAttackByDelay());
    }

    private IEnumerator EndAttackByDelay()
    {
        yield return new WaitForSeconds(attackInterval);
        enemyState.Behavior = EnemyState.BehaviorType.Idle;
    }
}
