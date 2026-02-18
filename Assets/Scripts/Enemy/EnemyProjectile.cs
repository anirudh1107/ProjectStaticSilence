using UnityEngine;

public class EnemyProjectile : MonoBehaviour, IDeflectable
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 10;

    private bool _isDeflected = false;
    private GameObject _owner; // Who shot this projectile (Enemy or Player)

    private float timer;
    private Vector2 direction;

    public void Initialize(Vector2 dir)
    {
        this.direction = dir.normalized;
        timer = lifeTime;
        _isDeflected = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        
        // Rotate bullet to face direction (Optional visual polish)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        Debug.Log("Projectile initialized with direction: " + direction);
    }

    private void Update()
    {
        // Simple translation
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        // Timer to return to pool if it hits nothing
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            ProjectilePool.Instance.ReturnProjectile(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Enemy"))
        {
            // Example damage logic
            // collision.GetComponent<PlayerHealth>()?.TakeDamage(damage);
            IDamagable damagable = collision.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage);
            }
            ProjectilePool.Instance.ReturnProjectile(this.gameObject);
        }
        
    }

    public bool OnDeflect(Vector2 newDirection, float newSpeed, GameObject deflector)
    {
        if (_isDeflected) return false; // Prevent double deflection in same frame

        _isDeflected = true;
        _owner = deflector; // New owner is the player (so it hurts enemies now)

        // 3. Visual flair (Change color to show it's deflected)
        GetComponent<SpriteRenderer>().color = Color.yellow;
        
        // 4. Update Rotation
        float angle = Mathf.Atan2(newDirection.y, newDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        return true;
    }
}