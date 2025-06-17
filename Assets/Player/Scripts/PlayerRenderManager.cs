using UnityEngine;

public interface IPlayerRenderManager
{
    public void SyncPlayerMoveState(float moveRate);
    public void OnJumpStarted();
    public void OnJumpEnded();
    public void OnDash();
    public void OnDashEnded();
    public void Play(string name);
    public void OnAttackEnded();
    public void OnGetHit();
    public void OnDeath();
}

public class PlayerRenderManager : MonoBehaviour, IPlayerRenderManager
{
    [SerializeField] private EffectData effectData;

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

    public void Play(string name) => animator.Play(name);

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

    public void OnDash()
    {
        animator.Play("Dash", 0);

        EffectController effect = Instantiate(effectData.Dash, transform);
        effect.transform.localPosition = new(0, -1, 0);

        if (playerState.FlipX) effect.transform.localScale =
                new Vector3(effect.transform.localScale.x, effect.transform.localScale.y, effect.transform.localScale.z * -1);
    }
        

    public void OnDashEnded() => ResetAnimationState();

    public void OnGetHit()
    {
        
    }

    public void OnDeath()
    {
        
    }
}
