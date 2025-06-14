using UnityEngine;

public interface IPlayerHealthManager
{

}

public class PlayerHealthManager : MonoBehaviour, IPlayerHealthManager
{
    private PlayerState playerState;
    private PlayerContext playerContext;

    public void Initialize(
        PlayerState playerState, 
        PlayerContext playerContext
        )
    {
        this.playerState = playerState;
        this.playerContext = playerContext;
    }
}
