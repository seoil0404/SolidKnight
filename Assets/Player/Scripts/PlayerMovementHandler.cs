using System.Collections;
using System.ComponentModel;
using UnityEngine;

public interface IPlayerMovementHandler
{

}

public class PlayerMovementHandler : MonoBehaviour, IPlayerMovementHandler
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private float jumpTime;

    private PlayerState playerState;
    private PlayerContext playerContext;
    private Rigidbody2D playerRigidBody;

    private bool isGround = true;

    private Coroutine jumpGravityCoroutine = null;
    private float gravityScale;

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
        HandleXAxisMove();
        HandleJump();
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
        if (Input.GetKeyUp(KeyData.Jump) && !isGround)
        {
            if(jumpGravityCoroutine != null) StopCoroutine(jumpGravityCoroutine);
            playerRigidBody.gravityScale = gravityScale;
        }
        else if (Input.GetKeyDown(KeyData.Jump) && isGround)
        {
            playerRigidBody.AddForceY(jumpPower);
            isGround = false;
            jumpGravityCoroutine = StartCoroutine(ScaleJumpGravity());
            playerContext.RenderManager.OnJumpStarted();
        }
    }

    private IEnumerator ScaleJumpGravity()
    {
        playerRigidBody.gravityScale = 0;
        yield return new WaitForSeconds(jumpTime);
        playerRigidBody.gravityScale = gravityScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Layer.Ground && !isGround)
        {
            playerContext.RenderManager.OnJumpEnded();
            isGround = true;
        }
    }
}
