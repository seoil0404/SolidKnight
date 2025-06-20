using System;

public class PlayerState
{
    public bool AllowMove = true;
    public bool IsDashing = false;
    public bool IsGround = true;
    public bool IsAttacking = false;
    public bool FlipX = false;
    public bool IsDeath = false;
}

public class PlayerContext
{
    public IPlayerRenderManager RenderManager;
    public IPlayerCombatHandler CombatHandler;
    public IPlayerController Controller;
    public IPlayerHealthManager HealthManager;
    public IPlayerMovementHandler MovementHandler;
}
