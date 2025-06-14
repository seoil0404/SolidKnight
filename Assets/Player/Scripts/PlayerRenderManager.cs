using UnityEngine;

public interface IPlayerRenderManager
{
    public void SyncPlayerMoveState(float moveRate);
    public void OnJumpStarted();
    public void OnJumpEnded();
    public void OnDash();
    public void OnDashEnded();
    public void OnAttack(PlayerCombatHandler.Combo combo);
    public void OnAttackEnded();
}

public class PlayerRenderManager : MonoBehaviour, IPlayerRenderManager
{
    private PlayerState playerState;
    private PlayerContext playerContext;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public void Initialize(
        PlayerState playerState, 
        PlayerContext playerContext, 
        Animator animator, 
        SpriteRenderer spriteRenderer
        )
    {
        this.playerState = playerState;
        this.playerContext = playerContext;
        this.animator = animator;
        this.spriteRenderer = spriteRenderer;
    }

    public void OnAttack(PlayerCombatHandler.Combo combo)
    {
        animator.Play(combo.AnimationName);
    }

    public void OnAttackEnded() => ResetAnimationState();

    public void OnJumpEnded() => ResetAnimationState();

    private void ResetAnimationState()
    {
        if (animator.GetBool("IsRun"))
            animator.Play("Run", 0);
        else animator.Play("Idle", 0);
    }

    public void OnJumpStarted()
    {
        animator.SetTrigger("OnJump");
    }

    public void SyncPlayerMoveState(float moveRate)
    {
        if(Mathf.Approximately(moveRate, 0f))
        {
            animator.SetBool("IsRun", false);

            return;
        }

        animator.SetBool("IsRun", true);

        if(moveRate > 0f)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        playerState.FlipX = spriteRenderer.flipX;
    }

    public void OnDash() =>
        animator.Play("Dash", 0);

    public void OnDashEnded() => ResetAnimationState();
}
