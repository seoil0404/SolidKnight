using UnityEngine;
using UnityEngine.TestTools;

public interface IEnemyRenderManager
{
    public void SyncEnemyMoveState(Vector2 moveRate);
}

public class EnemyRenderManager : MonoBehaviour, IEnemyRenderManager
{
    private EnemyContext enemyContext;
    private EnemyState enemyState;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public void Initialize(
        EnemyState enemyState, 
        EnemyContext enemyContext,
        SpriteRenderer spriteRenderer,
        Animator animator
        )
    {
        this.enemyState = enemyState;
        this.enemyContext = enemyContext;
        this.spriteRenderer = spriteRenderer;
        this.animator = animator;
    }

    public void SyncEnemyMoveState(Vector2 moveRate)
    {
        if(Mathf.Approximately(moveRate.x, 0))
        {
            animator.SetBool("IsWalk", false);

            return;
        }

        animator.SetBool("IsWalk", true);
    }

    public void HandleRender()
    {
        if (enemyState.Behavior == EnemyState.BehaviorType.Attack)
            return;

        if (transform.position.x < PlayerController.Transform.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        enemyState.FlipX = spriteRenderer.flipX;
    }
}
