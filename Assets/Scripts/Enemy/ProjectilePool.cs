using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool : MonoBehaviour
{
    public static ProjectilePool Instance { get; private set; }
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int poolSize = 10;

    private IObjectPool<GameObject> projectilePool;

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            InitializePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePool()
    {
        projectilePool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(projectilePrefab),
            actionOnGet: (obj) => obj.SetActive(true),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: false,
            defaultCapacity: poolSize,
            maxSize: poolSize * 2
        );
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetProjectile()
    {
        return projectilePool.Get();
    }

    public void ReturnProjectile(GameObject projectile)
    {
        projectilePool.Release(projectile);
    }
}
