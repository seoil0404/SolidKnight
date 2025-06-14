using UnityEngine;

public interface IPlayerController
{

}

[RequireComponent(typeof(PlayerHealthManager))]
[RequireComponent(typeof(PlayerMovementHandler))]
[RequireComponent(typeof(PlayerCombatHandler))]
[RequireComponent(typeof(PlayerRenderManager))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour, IPlayerController
{
    private PlayerHealthManager healthManager;
    private PlayerMovementHandler movementHandler;
    private PlayerCombatHandler combatHandler;
    private PlayerRenderManager renderManager;

    private void Awake()
    {
        PlayerState playerState = new();
        PlayerContext playerContext = new();

        healthManager = GetComponent<PlayerHealthManager>();
        movementHandler = GetComponent<PlayerMovementHandler>();
        combatHandler = GetComponent<PlayerCombatHandler>();
        renderManager = GetComponent<PlayerRenderManager>();

        playerContext.Controller = this;
        playerContext.HealthManager = healthManager;
        playerContext.MovementHandler = movementHandler;
        playerContext.CombatHandler = combatHandler;
        playerContext.RenderManager = renderManager;

        healthManager.Initialize(
            playerState: playerState,
            playerContext: playerContext
            );

        movementHandler.Initialize(
            playerState: playerState,
            playerContext: playerContext,
            playerRigidBody: GetComponent<Rigidbody2D>()
            );

        combatHandler.Initialize(
            playerState: playerState,
            playerContext: playerContext
            );

        renderManager.Initialize(
            playerState: playerState,
            playerContext: playerContext,
            animator: GetComponent<Animator>(),
            spriteRenderer: GetComponent<SpriteRenderer>()
            );
    }

    private void Update()
    {
        movementHandler.HandleMovement();
    }
}

public class PlayerState
{

}

public class PlayerContext
{
    public IPlayerRenderManager RenderManager;
    public IPlayerCombatHandler CombatHandler;
    public IPlayerController Controller;
    public IPlayerHealthManager HealthManager;
    public IPlayerMovementHandler MovementHandler;
}
