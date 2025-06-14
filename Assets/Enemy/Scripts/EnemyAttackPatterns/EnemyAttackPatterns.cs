using System.Collections;
using UnityEngine;

public interface IEnemyAttackPattern
{
    public void StartAttack(EnemyState enemyState, EnemyContext enemyContext);
}

public abstract class EnemyAttackPattern : IEnemyAttackPattern
{
    protected EnemyState enemyState;
    protected EnemyContext enemyContext;

    public void StartAttack(EnemyState enemyState, EnemyContext enemyContext)
    {
        this.enemyState = enemyState;
        this.enemyContext = enemyContext;

        StartAttack();
    }

    protected abstract void StartAttack();
}
