using UnityEngine;

public interface IPlayerCombatHandler
{

}

public class PlayerCombatHandler : MonoBehaviour, IPlayerCombatHandler
{
    private PlayerState playerState;
    private PlayerContext playerContext;

    private static readonly int maxCombo = 3;

    private int currentCombo = 0;

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
        if(Input.GetKeyDown(KeyData.Attack))
        {


            currentCombo = (currentCombo + 1) % maxCombo;
        }
    }

    public enum Combo
    {
        First,
        Second,
        Third
    }
}
