using System.Collections;
using System.ComponentModel;
using Unity.XR.GoogleVr;
using UnityEngine;

public interface IPlayerMovementHandler
{
    public void SetVelocity(Vector2 vector2);
}

public class PlayerMovementHandler : MonoBehaviour, IPlayerMovementHandler
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;

    [Header("Jump")]
    [SerializeField] private float jumpTime;

    [Header("Dash")]
    [SerializeField] private float dashTime;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashDelay;

    private PlayerState playerState;
    private PlayerContext playerContext;
    private Rigidbody2D playerRigidBody;

    private Coroutine jumpGravityCoroutine = null;
    private float gravityScale;

    private Coroutine dashCoroutine = null;

    public void Initialize(
        PlayerState playerState, 
        PlayerContext playerContext, 
        Rigidbody2D playerRigidBody
        )
    {
        this.playerRigidBody = playerRigidBody;
        this.playerState = playerState;
        this.playerContext = playerContext;

        gravityScale = this.playerRigidBody.gravityScale;
    }

    public void HandleMovement()
    {
        if (!playerState.AllowMove)
            return;

        HandleXAxisMove();
        HandleJump();
        HandleDash();
    }

    private void HandleXAxisMove()
    {
        float moveRate = 0f;

        if (Input.GetKey(KeyData.Right)) moveRate += moveSpeed;
        if (Input.GetKey(KeyData.Left)) moveRate -= moveSpeed;

        playerContext.RenderManager.SyncPlayerMoveState(moveRate);

        playerRigidBody.linearVelocityX = moveRate;
    }

    private void HandleJump()
    {
        if (Input.GetKeyUp(KeyData.Jump) && !playerState.IsGround)
        {
            if(jumpGravityCoroutine != null) StopCoroutine(jumpGravityCoroutine);
            playerRigidBody.gravityScale = gravityScale;
        }
        else if (Input.GetKeyDown(KeyData.Jump) && playerState.IsGround)
        {
            playerRigidBody.AddForceY(jumpPower);
            playerState.IsGround = false;
            jumpGravityCoroutine = StartCoroutine(ScaleJumpGravity());
            playerContext.RenderManager.OnJumpStarted();
        }
    }

    private void HandleDash()
    {
        if(Input.GetKeyDown(KeyData.Dash) && dashCoroutine == null)
        {
            dashCoroutine = StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        playerRigidBody.linearVelocityX = 0f;

        if (playerState.FlipX) playerRigidBody.AddForceX(-dashPower);
        else playerRigidBody.AddForceX(dashPower);

        playerState.IsDashing = true;
        playerState.AllowMove = false;

        playerContext.RenderManager.OnDash();

        yield return new WaitForSeconds(dashTime);

        playerState.AllowMove = true;
        playerState.IsDashing = false;

        playerRigidBody.linearVelocityX = 0;

        playerContext.RenderManager.OnDashEnded();

        yield return new WaitForSeconds(dashDelay);

        dashCoroutine = null;
    }

    private IEnumerator ScaleJumpGravity()
    {
        playerRigidBody.gravityScale = 0;

        yield return new WaitForSeconds(jumpTime);
        
        playerRigidBody.gravityScale = gravityScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Layer.Ground && !playerState.IsGround)
        {
            playerContext.RenderManager.OnJumpEnded();
            playerState.IsGround = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Layer.Ground && playerState.IsGround)
            playerState.IsGround = false;
    }

    public void SetVelocity(Vector2 vector2)
    {
        playerRigidBody.linearVelocity = vector2;
    }
}