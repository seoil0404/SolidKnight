using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IPlayerCombatHandler
{
    public void EndCombo();
    public Hitbox HitBoxPrefab { get; }
}

public class PlayerCombatHandler : MonoBehaviour, IPlayerCombatHandler
{
    [SerializeField] private float comboInterval;
    [SerializeField] private Hitbox hitboxPrefab;

    private PlayerState playerState;
    private PlayerContext playerContext;

    private int currentCombo = 0;
    private Coroutine resetComboCoroutine = null;

    private List<IPlayerAttackPattern> attackPatterns = new();

    public Hitbox HitBoxPrefab => hitboxPrefab;

    public void Initialize(
        PlayerState playerState,
        PlayerContext playerContext
        )
    {
        this.playerState = playerState;
        this.playerContext = playerContext;

        attackPatterns.Add(new FirstComboAttackPattern());
        attackPatterns.Add(new SecondComboAttackPattern());
        attackPatterns.Add(new ThirdComboAttackPattern());
    }

    public void HandleCombat()
    {
        if(Input.GetKeyDown(KeyData.Attack) && !playerState.IsAttacking && playerState.IsGround && !playerState.IsDashing)
        {
            if (resetComboCoroutine != null) StopCoroutine(resetComboCoroutine);

            attackPatterns[currentCombo].StartAttack(playerState, playerContext);

            playerState.IsAttacking = true;
            playerState.AllowMove = false;

            currentCombo = (currentCombo + 1) % attackPatterns.Count;
        }
    }

    public void EndCombo()
    {
        playerState.IsAttacking = false;
        playerContext.MovementHandler.SetVelocity(Vector2.zero);
        resetComboCoroutine = StartCoroutine(ResetCombo());
    }

    private IEnumerator ResetCombo()
    {
        yield return new WaitForSeconds(comboInterval);
        playerContext.RenderManager.OnAttackEnded();
        playerState.AllowMove = true;
        currentCombo = 0;
    }

    private interface IPlayerAttackPattern
    {
        public void StartAttack(PlayerState playerState, PlayerContext playerContext);
    }

    private abstract class PlayerAttackPattern : IPlayerAttackPattern
    {
        protected PlayerState playerState;
        protected PlayerContext playerContext;

        public void StartAttack(PlayerState playerState, PlayerContext playerContext)
        {
            this.playerState = playerState;
            this.playerContext = playerContext;

            StartAttack();
        }

        protected abstract void StartAttack();
    }

    private class FirstComboAttackPattern : PlayerAttackPattern
    {
        protected override void StartAttack()
        {
            playerContext.Controller.StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            playerContext.RenderManager.Play("Combo1");

            Hitbox currentHitBox = Instantiate(playerContext.CombatHandler.HitBoxPrefab, playerContext.MovementHandler.Transform).Initialize(Hitbox.Target.Enemy, new Vector2(3, 3), 1);
            if (playerState.FlipX) currentHitBox.transform.localPosition = new Vector3(-2, 0, 0);
            else currentHitBox.transform.localPosition = new Vector3(2, 0, 0);

            yield return new WaitForSeconds(0.25f);

            if (currentHitBox != null) Destroy(currentHitBox.gameObject);

            playerContext.CombatHandler.EndCombo();
        }
    }

    private class SecondComboAttackPattern : PlayerAttackPattern
    {
        protected override void StartAttack()
        {
            playerContext.Controller.StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            playerContext.RenderManager.Play("Combo2");
            
            Hitbox currentHitBox = Instantiate(playerContext.CombatHandler.HitBoxPrefab, playerContext.MovementHandler.Transform).Initialize(Hitbox.Target.Enemy, new Vector2(3, 3), 1);
            if(playerState.FlipX) currentHitBox.transform.localPosition = new Vector3(-2, 0, 0);
            else currentHitBox.transform.localPosition = new Vector3(2, 0, 0);

            yield return new WaitForSeconds(0.1f);

            if (currentHitBox != null) Destroy(currentHitBox.gameObject);

            playerContext.CombatHandler.EndCombo();
        }
    }

    private class ThirdComboAttackPattern : PlayerAttackPattern
    {
        protected override void StartAttack()
        {
            playerContext.Controller.StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            playerContext.RenderManager.Play("Combo3");

            Hitbox currentHitBox = Instantiate(playerContext.CombatHandler.HitBoxPrefab, playerContext.MovementHandler.Transform).Initialize(Hitbox.Target.Enemy, new Vector2(3, 3), 1);
            if (playerState.FlipX) currentHitBox.transform.localPosition = new Vector3(-2, 0, 0);
            else currentHitBox.transform.localPosition = new Vector3(2, 0, 0);

            yield return new WaitForSeconds(0.3f);

            if (currentHitBox != null) Destroy(currentHitBox.gameObject);

            playerContext.CombatHandler.EndCombo();
        }
    }
}
