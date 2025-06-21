using System.Collections;
using UnityEngine;

public interface IEnemyController
{
    public Coroutine StartCoroutine(IEnumerator routine);
    public void StopCoroutine(Coroutine routine);
    public void StopEnemy();
}

[RequireComponent(typeof(EnemyMovementHandler))]
[RequireComponent(typeof(EnemyHealthManager))]
[RequireComponent(typeof(EnemyRenderManager))]
[RequireComponent(typeof(EnemyCombatHandler))]
[RequireComponent(typeof(EnemySoundManager))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour, IEnemyController
{
    private EnemyHealthManager healthManager;
    private EnemyRenderManager renderManager;
    private EnemyCombatHandler combatHandler;
    private EnemyMovementHandler movementHandler;
    private EnemySoundManager soundManager;

    private bool isStop = false;

    public void StopEnemy() => isStop = true;

    private void Awake()
    {
        healthManager = GetComponent<EnemyHealthManager>();
        renderManager = GetComponent<EnemyRenderManager>();
        combatHandler = GetComponent<EnemyCombatHandler>();
        movementHandler = GetComponent<EnemyMovementHandler>();
        soundManager = GetComponent<EnemySoundManager>();

        EnemyContext enemyContext = new();
        EnemyState enemyState = new();

        enemyContext.Controller = this;
        enemyContext.HealthManager = healthManager;
        enemyContext.RenderManager = renderManager;
        enemyContext.CombatHandler = combatHandler;
        enemyContext.MovementHandler = movementHandler;
        enemyContext.SoundManager = soundManager;

        healthManager.Initialize(
            enemyState: enemyState,
            enemyContext: enemyContext
            );

        renderManager.Initialize(
            enemyState: enemyState,
            enemyContext: enemyContext,
            spriteRenderer: GetComponent<SpriteRenderer>(),
            animator: GetComponent<Animator>()
            );

        combatHandler.Initialize(
            enemyState: enemyState,
            enemyContext: enemyContext
            );

        movementHandler.Initialize(
            enemyState: enemyState,
            enemyContext: enemyContext,
            enemyRigidBody: GetComponent<Rigidbody2D>()
            );

        soundManager.Initialize(
            enemyState: enemyState,
            enemyContext: enemyContext
            );
    }

    private void Update()
    {
        if (isStop) return;
        if(GameManager.PlayerController.IsDeath)
        {
            StopEnemy();
            return;
        }

        movementHandler.HandleMovement();
        renderManager.HandleRender();
        combatHandler.HandleCombat();
    }
}
