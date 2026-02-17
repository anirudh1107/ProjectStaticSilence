using UnityEngine;

public class StateContext
{
    public PlayerManager playerManager;
    private State currentState;

    public void Initialize(PlayerManager playerManager, State initialState)
    {
        this.playerManager = playerManager;
        currentState = initialState;
        currentState.Enter();
    }

    public void ChangeState(State newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void ManagedUpdate()
    {
        currentState.Update();
    }
}
