using UnityEngine;

public class JumpState : State
{
    private StateContext stateContext;

    public JumpState(StateContext stateContext)
    {
        this.stateContext = stateContext;
    }
    public void Enter()
    {
        Debug.Log("Entered Jump State");
        stateContext.playerManager.rb.AddForce(Vector2.up * stateContext.playerManager.jumpForce, ForceMode2D.Impulse);
    }

    public void Update()
    {
        stateContext.playerManager.transform.Translate(stateContext.playerManager.movementInput * stateContext.playerManager.inAirSpeed * Time.deltaTime);
        if (stateContext.playerManager.rb.linearVelocity.y < 0.01f)
        {
            stateContext.ChangeState(stateContext.playerManager.fallState);
        }
        if (stateContext.playerManager.fireInput > 0 && 
            stateContext.playerManager.gunStamina.CanFire(stateContext.playerManager.inAirGunStaminaCost))
        {
            Debug.Log("Fire in the air!");
            stateContext.playerManager.FireSoundWave(stateContext.playerManager.GroundPoint);
            stateContext.ChangeState(stateContext.playerManager.fireJumpState);
        }
    }

    public void Exit()
    {
    }
    
}
