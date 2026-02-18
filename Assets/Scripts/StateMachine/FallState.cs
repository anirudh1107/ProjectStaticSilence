using UnityEngine;

public class FallState : State
{
    private StateContext stateContext;

    public FallState(StateContext stateContext)
    {
        this.stateContext = stateContext;
    }
    public void Enter()
    {
        Debug.Log("Entered Fall State");
    }

    public void Update()
    {
        if (Mathf.Abs(stateContext.playerManager.rb.linearVelocity.y) <= 0.01)
        {
            stateContext.ChangeState(stateContext.playerManager.idleState);
        }
         if (stateContext.playerManager.fireInput > 0 && 
            stateContext.playerManager.gunStamina.CanFire(stateContext.playerManager.inAirGunStaminaCost))
        {
            Debug.Log("Fire in the air!");
            stateContext.playerManager.FireSoundWave(stateContext.playerManager.GunPoint);
            stateContext.ChangeState(stateContext.playerManager.fireJumpState);
        }
        stateContext.playerManager.transform.Translate(stateContext.playerManager.movementInput * stateContext.playerManager.inAirSpeed * Time.deltaTime);
    }

    public void Exit()
    {
    }
    
}
