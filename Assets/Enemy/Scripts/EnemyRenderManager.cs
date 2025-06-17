using DG.Tweening;
using UnityEngine;
using UnityEngine.TestTools;

public interface IEnemyRenderManager
{
    public void SyncEnemyMoveState(Vector2 moveRate);
    public void PlayAnimation(string animationName);
    public void ResetAnimationState();
    public void FadeColor(Color startColor, Color endColor, float time);
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

    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName, 0);
    }

    public void ResetAnimationState()
    {
        if (animator.GetBool("IsWalk"))
            animator.Play("Walk", 0);
        else animator.Play("Idle", 0);
    }

    public void FadeColor(Color startColor, Color endColor, float time)
    {
        spriteRenderer.DOKill();
        spriteRenderer.color = startColor;
        spriteRenderer.DOColor(endColor, time);
    }
}
