using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;

public interface IPlayerCombatHandler
{
    public void EndCombo();
    public bool IsParring();
    public void OnParringSucces();
    public void StopAttack();
    public Hitbox HitBoxPrefab { get; }
}

public class PlayerCombatHandler : MonoBehaviour, IPlayerCombatHandler
{
    [Header("Combo")]
    [SerializeField] private float comboInterval;
    [Header("Parring")]
    [SerializeField] private float parringInterval;
    [SerializeField] private float parringTime;
    [Header("Prefab")]
    [SerializeField] private Hitbox hitboxPrefab;

    private PlayerState playerState;
    private PlayerContext playerContext;

    private int currentCombo = 0;
    private Coroutine resetComboCoroutine = null;

    private Coroutine parringCooldown = null;

    private List<IPlayerAttackPattern> attackPatterns = new();
    private IPlayerAttackPattern currentAttackPattern;

    private bool isParring = false;

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
        if (Input.GetKeyDown(KeyData.Parring) && playerState.IsGround)
            UseParring();
        if(Input.GetKeyDown(KeyData.Attack) && !playerState.IsAttacking && playerState.IsGround && !playerState.IsDashing)
        {
            if (resetComboCoroutine != null) StopCoroutine(resetComboCoroutine);

            attackPatterns[currentCombo].StartAttack(playerState, playerContext);
            currentAttackPattern = attackPatterns[currentCombo];

            playerState.IsAttacking = true;
            playerState.AllowMove = false;

            currentCombo = (currentCombo + 1) % attackPatterns.Count;
        }
    }

    private void UseParring()
    {
        if (parringCooldown != null)
            return;

        playerContext.MovementHandler.SetVelocity(Vector2.zero);
        playerContext.RenderManager.OnParring();
        StartCoroutine(Parring());
        parringCooldown = StartCoroutine(ParringCooldown());
    }

    private IEnumerator Parring()
    {
        isParring = true;
        yield return new WaitForSeconds(parringTime);
        isParring = false;
    }

    private IEnumerator ParringCooldown()
    {
        yield return new WaitForSeconds(parringInterval);
        parringCooldown = null;
    }


    public void EndCombo()
    {
        playerState.IsAttacking = false;
        playerContext.MovementHandler.SetVelocity(Vector2.zero);
        playerState.AllowMove = true;
        resetComboCoroutine = StartCoroutine(ResetCombo());
    }

    private IEnumerator ResetCombo()
    {
        yield return new WaitForSeconds(comboInterval);
        
        if (playerState.IsDeath) yield break;

        playerContext.RenderManager.OnAttackEnded();
        currentCombo = 0;
    }

    public bool IsParring() =>
        isParring && playerContext.RenderManager.IsPlaying("Parring");

    public void OnParringSucces()
    {
        StopCoroutine(parringCooldown);
        playerContext.MovementHandler.OnParringSucces();
        playerContext.RenderManager.OnParringSucces();
        playerContext.SoundManager.PlaySound(PlayerSoundManager.SoundType.Parring, 0.5f);

        parringCooldown = null;
    }

    public void StopAttack()
    {
        if(currentAttackPattern != null) currentAttackPattern.StopAttack();
    }
        

    private interface IPlayerAttackPattern
    {
        public void StartAttack(PlayerState playerState, PlayerContext playerContext);
        public void StopAttack();
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

        public abstract void StopAttack();

        protected abstract void StartAttack();
    }

    private class FirstComboAttackPattern : PlayerAttackPattern
    {
        private Coroutine currentCoroutine = null;
        private Hitbox currentHitBox = null;

        public override void StopAttack()
        {
            if(currentCoroutine != null) playerContext.Controller.StopCoroutine(currentCoroutine);
            if(currentHitBox != null) Destroy(currentHitBox.gameObject);
            playerContext.CombatHandler.EndCombo();
        }

        protected override void StartAttack()
        {
            currentCoroutine = playerContext.Controller.StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            playerContext.RenderManager.Play("Combo1");
            playerContext.MovementHandler.SetVelocity(Vector2.zero);
            playerContext.SoundManager.PlaySound(PlayerSoundManager.SoundType.Slash);

            currentHitBox = Instantiate(playerContext.CombatHandler.HitBoxPrefab, playerContext.MovementHandler.Transform).Initialize(Hitbox.Target.Enemy, new Vector2(4.5f, 3), 1);

            if (playerState.FlipX) currentHitBox.transform.localPosition = new Vector3(-2, 0, 0);
            else currentHitBox.transform.localPosition = new Vector3(2, 0, 0);

            yield return new WaitForSeconds(0.25f);

            if (currentHitBox != null) Destroy(currentHitBox.gameObject);

            playerContext.CombatHandler.EndCombo();
        }
    }

    private class SecondComboAttackPattern : PlayerAttackPattern
    {
        private Coroutine currentCoroutine = null;
        private Hitbox currentHitBox = null;

        public override void StopAttack()
        {
            if (currentCoroutine != null) playerContext.Controller.StopCoroutine(currentCoroutine);
            if (currentHitBox != null) Destroy(currentHitBox.gameObject);
            playerContext.CombatHandler.EndCombo();
        }

        protected override void StartAttack()
        {
            currentCoroutine = playerContext.Controller.StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            playerContext.RenderManager.Play("Combo2");
            playerContext.MovementHandler.SetVelocity(Vector2.zero);
            playerContext.SoundManager.PlaySound(PlayerSoundManager.SoundType.Slash);

            currentHitBox = Instantiate(playerContext.CombatHandler.HitBoxPrefab, playerContext.MovementHandler.Transform).Initialize(Hitbox.Target.Enemy, new Vector2(4.5f, 3), 1);
            if(playerState.FlipX) currentHitBox.transform.localPosition = new Vector3(-2, 0, 0);
            else currentHitBox.transform.localPosition = new Vector3(2, 0, 0);

            yield return new WaitForSeconds(0.1f);

            if (currentHitBox != null) Destroy(currentHitBox.gameObject);

            playerContext.CombatHandler.EndCombo();
        }
    }

    private class ThirdComboAttackPattern : PlayerAttackPattern
    {
        private Coroutine currentCoroutine = null;
        private Hitbox currentHitBox = null;

        public override void StopAttack()
        {
            if (currentCoroutine != null) playerContext.Controller.StopCoroutine(currentCoroutine);
            if (currentHitBox != null) Destroy(currentHitBox.gameObject);
            playerContext.CombatHandler.EndCombo();
        }

        protected override void StartAttack()
        {
            currentCoroutine = playerContext.Controller.StartCoroutine(Attack());
        }

        private IEnumerator Attack()
        {
            playerContext.RenderManager.Play("Combo3");
            playerContext.MovementHandler.SetVelocity(Vector2.zero);
            playerContext.SoundManager.PlaySound(PlayerSoundManager.SoundType.Slash);

            currentHitBox = Instantiate(playerContext.CombatHandler.HitBoxPrefab, playerContext.MovementHandler.Transform).Initialize(Hitbox.Target.Enemy, new Vector2(4.5f, 3), 1);
            if (playerState.FlipX) currentHitBox.transform.localPosition = new Vector3(-2, 0, 0);
            else currentHitBox.transform.localPosition = new Vector3(2, 0, 0);

            yield return new WaitForSeconds(0.3f);

            if (currentHitBox != null) Destroy(currentHitBox.gameObject);

            playerContext.CombatHandler.EndCombo();
        }
    }
}
