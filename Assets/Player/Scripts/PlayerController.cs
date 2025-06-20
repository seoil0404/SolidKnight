using System.Collections;
using UnityEngine;

public interface IPlayerController
{
    public Coroutine StartCoroutine(IEnumerator routine);
    public void StopCoroutine(Coroutine routine);
    public void StopPlayer();
    public void StopPlayer(float stopTime);
    public void ResumePlayer();
    public bool IsDeath { get; }
    public Transform TrackingSubject { get; }
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
    [SerializeField] private Transform trakingSubject;

    private PlayerHealthManager healthManager;
    private PlayerMovementHandler movementHandler;
    private PlayerCombatHandler combatHandler;
    private PlayerRenderManager renderManager;

    private bool isStop = false;

    private PlayerState playerState = new();
    PlayerContext playerContext = new();

    public static Transform Transform { get; private set; }

    public bool IsDeath => playerState.IsDeath;

    public Transform TrackingSubject => trakingSubject;

    private void Awake()
    {
        if (Transform != null)
        {
            Destroy(Transform.gameObject);
        }

        DontDestroyOnLoad(gameObject);
        Transform = transform;

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

        GameManager.PlayerController = this;
    }

    private void Update()
    {
        if(isStop) return;

        movementHandler.HandleMovement();
        combatHandler.HandleCombat();
        renderManager.UpdateHealthSlider();
    }

    public void StopPlayer()
    {
        movementHandler.SetVelocity(Vector2.zero);
        combatHandler.StopAttack();

        isStop = true;
    }

    public void ResumePlayer() => isStop = false;

    public void StopPlayer(float stopTime)
    {
        StartCoroutine(StopPlayerByTime(stopTime));
    }

    private IEnumerator StopPlayerByTime(float stopTime)
    {
        StopPlayer();
        yield return new WaitForSeconds(stopTime);
        ResumePlayer();
    }
}