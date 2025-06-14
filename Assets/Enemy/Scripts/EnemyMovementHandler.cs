using System;
using UnityEngine;

public interface IEnemyMovementHandler
{

}

public class EnemyMovementHandler : MonoBehaviour, IEnemyMovementHandler
{
    [SerializeField] private MovementType movementType;
    [SerializeField] private float maxChaseRange;
    [SerializeField] private float minChaseRange;
    [SerializeField] private float chaseSpeed;

    private EnemyContext enemyContext;
    private EnemyState enemyState;
    private Rigidbody2D enemyRigidBody;

    public void Initialize(
        EnemyState enemyState,
        EnemyContext enemyContext,
        Rigidbody2D enemyRigidBody
        )
    {
        this.enemyState = enemyState;
        this.enemyContext = enemyContext;
        this.enemyRigidBody = enemyRigidBody;
    }

    public void HandleMovement()
    {
        if (enemyState.Behavior == EnemyState.BehaviorType.Attack)
            return;

        if (Vector2.Distance(transform.position, PlayerController.Transform.position) > maxChaseRange || 
            Vector2.Distance(transform.position, PlayerController.Transform.position) < minChaseRange
            )
        {
            enemyState.Behavior = EnemyState.BehaviorType.Idle;
            enemyRigidBody.linearVelocity = Vector2.zero;
            enemyContext.RenderManager.SyncEnemyMoveState(Vector2.zero);

            return;
        }

        enemyState.Behavior = EnemyState.BehaviorType.Chase;

        Vector2 moveRate = PlayerController.Transform.position - transform.position;
        if (movementType != MovementType.Flying) moveRate.y = 0;

        moveRate.Normalize();
        moveRate *= chaseSpeed;

        enemyRigidBody.linearVelocity = moveRate;
        enemyContext.RenderManager.SyncEnemyMoveState(moveRate);
    }

    [Serializable]
    public enum MovementType
    {
        Ground, Flying
    }
}
