using UnityEngine;

public interface IEnemyController
{

}

[RequireComponent(typeof(EnemyMovementHandler))]
[RequireComponent(typeof(EnemyHealthManager))]
[RequireComponent(typeof(EnemyRenderManager))]
[RequireComponent(typeof(EnemyCombatHandler))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour, IEnemyController
{
    private EnemyHealthManager healthManager;
    private EnemyRenderManager renderManager;
    private EnemyCombatHandler combatHandler;
    private EnemyMovementHandler movementHandler;

    private void Awake()
    {
        healthManager = GetComponent<EnemyHealthManager>();
        renderManager = GetComponent<EnemyRenderManager>();
        combatHandler = GetComponent<EnemyCombatHandler>();
        movementHandler = GetComponent<EnemyMovementHandler>();

        EnemyContext enemyContext = new();
        EnemyState enemyState = new();

        enemyContext.Controller = this;
        enemyContext.HealthManager = healthManager;
        enemyContext.RenderManager = renderManager;
        enemyContext.CombatHandler = combatHandler;
        enemyContext.MovementHandler = movementHandler;

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
    }

    private void Update()
    {
        movementHandler.HandleMovement();
        renderManager.HandleRender();
        combatHandler.HandleCombat();
    }
}
