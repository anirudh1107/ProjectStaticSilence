using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{

    public int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public int CurrentHealth { get => currentHealth; set => currentHealth = value; }
    private enum State { Patrol, Chase, Shoot }

    [Header("Settings")]
    [SerializeField] private int maxHealth; 
    [SerializeField]private int currentHealth;
    [SerializeField] private State currentState;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float chaseSpeed = 5f;
    
    [Header("Detection")]
    [SerializeField] private float detectionRadius = 6f;
    [SerializeField] private float shootingRadius = 3f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Patrol")]
    [SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Combat")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    private float nextFireTime;

    private Transform playerTransform;
    private Vector2 lastHeardSoundLocation;
    private bool investigatingSound = false;



    private void Start()
    {
        maxHealth = 10;
        currentHealth = maxHealth;
        // Find player strictly by Tag to avoid heavy GetComponent calls in Update
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj) playerTransform = playerObj.transform;
    }

    private void Update()
    {
        if (playerTransform == null) return;

        float distToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        // State Transitions
        CheckTransitions(distToPlayer);

        // State Execution
        ExecuteState(distToPlayer);
    }

    // --- State Management ---

    private void CheckTransitions(float distToPlayer)
    {
        switch (currentState)
        {
            case State.Patrol:
                if (distToPlayer <= detectionRadius || investigatingSound)
                {
                    currentState = State.Chase;
                }
                break;

            case State.Chase:
                if (distToPlayer <= shootingRadius)
                {
                    currentState = State.Shoot;
                }
                else if (distToPlayer > detectionRadius * 1.5f && !investigatingSound)
                {
                    // Player escaped far enough
                    currentState = State.Patrol;
                }
                break;

            case State.Shoot:
                if (distToPlayer > shootingRadius)
                {
                    currentState = State.Chase;
                }
                break;
        }
    }

    private void ExecuteState(float distToPlayer)
    {
        switch (currentState)
        {
            case State.Patrol:
                PatrolBehavior();
                break;
            case State.Chase:
                ChaseBehavior();
                break;
            case State.Shoot:
                ShootBehavior();
                break;
        }
    }

    // --- Behaviors ---

    private void PatrolBehavior()
    {
        if (waypoints.Length == 0) return;

        Transform targetWP = waypoints[currentWaypointIndex];
        MoveTowards(targetWP.position, moveSpeed);

        if (Vector2.Distance(transform.position, targetWP.position) < 0.5f)
        {
            // Cycle through waypoints (0 -> 1 -> 0)
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private void ChaseBehavior()
    {
        Vector2 targetPos = playerTransform.position;

        // If checking a sound, go to the sound location, not the player
        if (investigatingSound)
        {
            targetPos = lastHeardSoundLocation;
            if (Vector2.Distance(transform.position, targetPos) < 0.5f)
            {
                investigatingSound = false; // Arrived at sound source, nothing found
            }
        }

        MoveTowards(targetPos, chaseSpeed);
    }

    private void ShootBehavior()
    {
        // Stop moving while shooting (optional)
        // Look at player
        FaceTarget(playerTransform.position);

        if (Time.time >= nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void FireProjectile()
    {
        GameObject proj = ProjectilePool.Instance.GetProjectile();
        proj.transform.position = firePoint.position;
        proj.transform.rotation = firePoint.rotation;

        Vector2 dir = (playerTransform.position - firePoint.position).normalized;
        proj.GetComponent<EnemyProjectile>().Initialize(dir);
    }

    // --- Helpers ---

    private void MoveTowards(Vector2 target, float speed)
    {
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        FaceTarget(target);
    }

    private void FaceTarget(Vector2 target)
    {
        // Assuming sprite faces Right by default
        if (target.x > transform.position.x)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }

    // Public method to be called by Player script when making noise
    public void AlertEnemy(Vector2 soundLocation)
    {
        if (currentState == State.Patrol)
        {
            lastHeardSoundLocation = soundLocation;
            investigatingSound = true;
            // Force state switch
            currentState = State.Chase; 
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log("Enemy took damage! Current health: " + currentHealth);
        if (currentHealth <= 0)
        {
            CheckForDeath();
        }
    }

    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(maxHealth, currentHealth + healAmount);
        Debug.Log("Enemy healed! Current health: " + currentHealth);
    }

    public void CheckForDeath()
    {
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Visual Debugging
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootingRadius);
    }
}