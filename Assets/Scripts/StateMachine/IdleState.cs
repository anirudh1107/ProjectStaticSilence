using UnityEngine;

public class IdleState : State
{
    private StateContext stateContext;

    public IdleState(StateContext stateContext)
    {
        this.stateContext = stateContext;
    }
	public void Enter()
    {
        Debug.Log("Entered Idle State");
    }

	public void Update()
    {
        if (stateContext.playerManager.isMoving)
        {
            stateContext.ChangeState(stateContext.playerManager.moveState);
        }
        if (stateContext.playerManager.jumpInput > 0)
        {
            stateContext.ChangeState(stateContext.playerManager.jumpState);
        }
        if (stateContext.playerManager.fireInput > 0 && 
            stateContext.playerManager.gunStamina.CanFire(stateContext.playerManager.groundGunStaminaCost))
        {
            Debug.Log("Fire");
            stateContext.playerManager.FireSoundWave(stateContext.playerManager.GunPoint);
            stateContext.playerManager.fireInput = 0;
            stateContext.playerManager.gunStamina.UseStamina(stateContext.playerManager.groundGunStaminaCost);
        }
    }

	public void Exit()
	{
	}
}
