using DG.Tweening;
using UnityEngine;
using UnityEngine.TestTools;

public interface IEnemyRenderManager
{
    public void SyncEnemyMoveState(Vector2 moveRate);
    public void PlayAnimation(string animationName);
    public void ResetAnimationState();
    public void FadeColor(Color startColor, Color endColor, float time);
    public void ShakeCamera(float duration, float strength = 2, int vibrato = 5, float randomness = 90, bool snapping = false, bool fadeOut = true);
    public void OnDeath();
}

public class EnemyRenderManager : MonoBehaviour, IEnemyRenderManager
{
    [SerializeField] private EffectData effectData;

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

    public void ShakeCamera(
        float duration, 
        float strength = 2f, 
        int vibrato = 20, 
        float randomness = 90, 
        bool snapping = false, 
        bool fadeOut = true
        )
    {
        CinemachineManager.Instance.ShakeCamera(
            duration, 
            strength, 
            vibrato, 
            randomness, 
            snapping, 
            fadeOut
            );
    }

    public void OnDeath()
    {
        animator.Play("Death", 0);
        CinemachineManager.Instance.ShakeCamera(
            duration: 10,
            strength: 1,
            vibrato: 20,
            randomness: 90,
            snapping: false,
            fadeOut: false);
        Instantiate(effectData.Death).transform.position = transform.position;
    }
}
