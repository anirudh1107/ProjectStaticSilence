using UnityEngine;

public class FireJumpState : State
{
    private StateContext stateContext;

    public FireJumpState(StateContext stateContext)
    {
        this.stateContext = stateContext;
    }
    public void Enter()
    {
        Debug.Log("Entered Fire Jump State");
        stateContext.playerManager.fireInput = 0;
        stateContext.playerManager.rb.AddForce(Vector2.up * stateContext.playerManager.fireJumpForce, ForceMode2D.Impulse);
        stateContext.playerManager.gunStamina.UseStamina(stateContext.playerManager.inAirGunStaminaCost);
    }

    public void Update()
    {
        if (stateContext.playerManager.jumpInput <= 0)
        {
            stateContext.ChangeState(stateContext.playerManager.fallState);
        }
    }

    public void Exit()
    {
    }
    
}
