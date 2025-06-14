using System.Collections;
using UnityEngine;

public class MutantPatternData : EnemyPatternData
{
    public override void Initialize()
    {
        AttackPatterns.Add(new JumpAttackPattern());
        AttackPatterns.Add(new PunchAttackPattern());
        AttackPatterns.Add(new DropAttackPattern());
    }

    public class JumpAttackPattern : EnemyAttackPattern
    {
        protected override void StartAttack()
        {
            enemyContext.Controller.StartCorutine(Attack());
        }

        private IEnumerator Attack()
        {
            enemyContext.RenderManager.PlayAnimation("JumpAttack");

            yield return new WaitForSeconds(0.25f);

            enemyContext.MovementHandler.AddForce(new Vector2(0, 750));
            float playerPositionX = PlayerController.Transform.position.x;

            yield return new WaitForSeconds(0.75f);

            enemyContext.MovementHandler.AddForce(
                new Vector2(
                    (playerPositionX - enemyContext.MovementHandler.Position.x) * 200,
                    -1500
                    )
                );

            yield return new WaitForSeconds(0.35f);

            enemyContext.MovementHandler.SetVelocity(Vector2.zero);

            enemyContext.CombatHandler.OnEndAttack();
        }
    }
    public class PunchAttackPattern : EnemyAttackPattern
    {
        protected override void StartAttack()
        {
            enemyContext.Controller.StartCorutine(Attack());
        }

        private IEnumerator Attack()
        {
            float moveRate = 1;
            if (!enemyState.FlipX) moveRate *= -1;

            enemyContext.RenderManager.PlayAnimation("Punch");

            yield return new WaitForSeconds(0.475f);

            enemyContext.MovementHandler.SetVelocity(new Vector2(moveRate * 35, 0));

            yield return new WaitForSeconds(0.1f);

            enemyContext.MovementHandler.SetVelocity(new Vector2(0, 0));

            yield return new WaitForSeconds(0.35f);

            enemyContext.MovementHandler.SetVelocity(new Vector2(moveRate * 35, 0));

            yield return new WaitForSeconds(0.1f);

            enemyContext.MovementHandler.SetVelocity(new Vector2(0, 0));

            enemyContext.CombatHandler.OnEndAttack();
        }
    }
    public class DropAttackPattern : EnemyAttackPattern
    {
        protected override void StartAttack()
        {
            enemyContext.Controller.StartCorutine(Attack());
        }

        private IEnumerator Attack()
        {
            enemyContext.RenderManager.PlayAnimation("DropAttack");
            enemyContext.MovementHandler.AddForce((PlayerController.Transform.position - enemyContext.MovementHandler.Position + new Vector3(0, 7)) * 600);
            
            enemyContext.MovementHandler.UseGravity = false;

            yield return new WaitForSeconds(0.1f);

            enemyContext.MovementHandler.SetVelocity(Vector2.zero);

            yield return new WaitForSeconds(0.45f);

            enemyContext.MovementHandler.UseGravity = true;
            enemyContext.MovementHandler.SetVelocity(Vector2.down * 100);

            yield return new WaitForSeconds(0.1f);

            enemyContext.CombatHandler.OnEndAttack();
        }
    }
}