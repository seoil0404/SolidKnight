using System;
using System.Collections;
using UnityEngine;

public interface IEnemyMovementHandler
{
    public Vector3 Position { get; }
    public Transform Transform { get; }
    public bool UseGravity { set; }
    public void AddForce(Vector2 moveRate);
    public void Translate(Vector2 position);
    public void SetVelocity(Vector2 velocity);
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

    private float initialGravity;

    public Vector3 Position => transform.position;
    public Transform Transform => transform;
    public bool UseGravity
    {
        set
        {
            if (value)
                enemyRigidBody.gravityScale = initialGravity;
            else 
                enemyRigidBody.gravityScale = 0;
        }
    }

    public void Initialize(
        EnemyState enemyState,
        EnemyContext enemyContext,
        Rigidbody2D enemyRigidBody
        )
    {
        this.enemyState = enemyState;
        this.enemyContext = enemyContext;
        this.enemyRigidBody = enemyRigidBody;
        initialGravity = this.enemyRigidBody.gravityScale;
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

    public void AddForce(Vector2 moveRate)
    {
        enemyRigidBody.AddForce(moveRate);
    }

    public void SetVelocity(Vector2 velocity)
    {
        enemyRigidBody.linearVelocity = velocity;
    }

    public void Translate(Vector2 position)
    {
        enemyRigidBody.MovePosition(position);
    }

    [Serializable]
    public enum MovementType
    {
        Ground, Flying
    }
}
