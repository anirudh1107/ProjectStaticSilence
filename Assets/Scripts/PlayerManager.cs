using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public Vector2 movementInput;
    public float jumpInput;
    public float fireInput;
    public bool isMoving;
    public float movementSpeed = 5f;
    public float jumpForce = 5f;
    public float fireJumpForce = 2f;
    public float inAirSpeed = 2f;
    public float inAirGunStaminaCost = 20f;
    public float groundGunStaminaCost = 10f;
    public int facingDirection = 1; // 1 for right, -1 for left
    public LayerMask interactiveLayer;

    public Rigidbody2D rb;
    public State idleState;
    public State moveState;
    public State jumpState;
    public State fallState;
    public State fireJumpState;
    public GunStamina gunStamina;

    [SerializeField] private Transform GunPoint;
    [SerializeField]private Transform GroundPoint;
    private StateContext stateContext;
    

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            isMoving = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            isMoving = false;
            movementInput = Vector2.zero;
        }
        movementInput = context.ReadValue<Vector2>();

    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            jumpInput = context.ReadValue<float>();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            jumpInput = 0;
        }
        
    }

    public void OnFire(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            fireInput = context.ReadValue<float>();
        }
         if (context.phase == InputActionPhase.Canceled)
        {
            fireInput = 0;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            PerformAction();
        }
    }

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        stateContext = new StateContext();
        idleState = new IdleState(stateContext);
        moveState = new MoveState(stateContext);
        jumpState = new JumpState(stateContext);
        fallState = new FallState(stateContext);
        fireJumpState = new FireJumpState(stateContext);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        stateContext.Initialize(this, idleState);
        gunStamina = GetComponent<GunStamina>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageFacingDirection();
        stateContext.ManagedUpdate();
    }

    private void ManageFacingDirection()
    {
        if (movementInput.x > 0 && facingDirection != 1)
        {
            facingDirection = 1;
            transform.localScale = new Vector3(facingDirection, 1, 1);
        }
        else if (movementInput.x < 0 && facingDirection != -1)
        {
            facingDirection = -1;
            transform.localScale = new Vector3(facingDirection, 1, 1);
        }
    }

    private void PerformAction()
    {
        Debug.Log("Performing action!");
        Collider2D[] interactables = Physics2D.OverlapCircleAll(GroundPoint.transform.position, 5f, interactiveLayer);
            foreach (Collider2D collider in interactables)
            {
                Debug.Log("Performing action inside!");
                if (collider.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    interactable.Interact(this);
                }
            }
    }

    public void FillStamina(float amount)
    {
        gunStamina.RegenerateStamina(amount);
    }
}
