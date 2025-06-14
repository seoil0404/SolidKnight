using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public interface IPlayerCombatHandler
{

}

public class PlayerCombatHandler : MonoBehaviour, IPlayerCombatHandler
{
    [SerializeField] private ComboData comboData;
    [SerializeField] private float comboInterval;

    private PlayerState playerState;
    private PlayerContext playerContext;

    private int currentCombo = 0;
    private Coroutine resetComboCoroutine = null;

    public void Initialize(
        PlayerState playerState,
        PlayerContext playerContext
        )
    {
        this.playerState = playerState;
        this.playerContext = playerContext;
    }

    public void HandleCombat()
    {
        if(Input.GetKeyDown(KeyData.Attack) && !playerState.IsAttacking && playerState.IsGround && !playerState.IsDashing)
        {
            if (resetComboCoroutine != null) StopCoroutine(resetComboCoroutine);

            playerContext.RenderManager.OnAttack(comboData.List[currentCombo]);

            if (playerState.FlipX) playerContext.MovementHandler.SetVelocity(-comboData.List[currentCombo].MoveRate);
            else playerContext.MovementHandler.SetVelocity(comboData.List[currentCombo].MoveRate);

            playerState.IsAttacking = true;
            playerState.AllowMove = false;

            currentCombo = (currentCombo + 1) % comboData.List.Count;
        }
    }

    private void EndCombo()
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

    [Serializable]
    public class Combo
    {
        public string AnimationName;
        public Vector2 MoveRate;
    }
}
