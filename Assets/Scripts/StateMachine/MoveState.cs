using UnityEngine;

public class MoveState : State
{
    private StateContext stateContext;

    public MoveState(StateContext stateContext)
    {
        this.stateContext = stateContext;
    }
    public void Enter()
    {
        Debug.Log("Entered Move State");
    }

    public void Update()
    {
        if (!stateContext.playerManager.isMoving)
        {
            stateContext.ChangeState(stateContext.playerManager.idleState);
        }
        if (stateContext.playerManager.jumpInput > 0)
        {
            stateContext.ChangeState(stateContext.playerManager.jumpState);
        }
        if (stateContext.playerManager.fireInput > 0 && 
            stateContext.playerManager.gunStamina.CanFire(stateContext.playerManager.groundGunStaminaCost))
        {
            Debug.Log("Fire");
            stateContext.playerManager.fireInput = 0;
            stateContext.playerManager.gunStamina.UseStamina(stateContext.playerManager.groundGunStaminaCost);
        }

        stateContext.playerManager.transform.Translate(stateContext.playerManager.movementInput * stateContext.playerManager.movementSpeed * Time.deltaTime);
    }

    public void Exit()
    {
    }
}
