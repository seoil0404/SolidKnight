using System.Collections;
using UnityEngine;

public interface IPlayerController
{
    public Coroutine StartCoroutine(IEnumerator routine);
    public void StopCoroutine(Coroutine routine);
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
    public static Transform Transform { get; private set; }

    private PlayerHealthManager healthManager;
    private PlayerMovementHandler movementHandler;
    private PlayerCombatHandler combatHandler;
    private PlayerRenderManager renderManager;

    private void Awake()
    {
        if (Transform != null)
        {
            Destroy(gameObject);
            return;
        }
        
        Transform = transform;

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
        combatHandler.HandleCombat();
    }
}