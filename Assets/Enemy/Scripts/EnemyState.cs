using UnityEngine;

public class EnemyState
{
    public BehaviorType Behavior = BehaviorType.Idle;
    public bool FlipX = false;

    public enum BehaviorType
    {
        Idle,
        Chase,
        Attack
    }
}


public class EnemyContext
{
    public IEnemyRenderManager RenderManager;
    public IEnemyMovementHandler MovementHandler;
    public IEnemyCombatHandler CombatHandler;
    public IEnemyHealthManager HealthManager;
    public IEnemyController Controller;
}
