using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class DriftEnemy : MonoBehaviour
{
    [SerializeField] private GameObject drifterPrefab;
    [SerializeField] private float destroyMargin = 0.2f;

    private float health;
    private Rigidbody2D rb;
    private Camera mainCamera;
    private EnemySpawner enemySpawner;
    private PooledObject pooledObject;
    private PlayerData playerScript;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        pooledObject = GetComponent<PooledObject>();

        if (drifterPrefab == null) drifterPrefab = gameObject;

        rb.gravityScale = 0f;
        rb.linearDamping = 0f;
        rb.angularDamping = 0f;

    }

    private void OnEnable()
    {
        if(rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
    }
    public void SetSpawner(EnemySpawner e)
    {
        enemySpawner = e;
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0f)
        {
            Die();
        }
        else
        {
            //if not break or die give feedback
        }
    }

    private void Die()
    {
        if (enemySpawner != null)
            enemySpawner.NotifyAsteroidDestroyed();

        DespawnSelf();
    }

    private void DespawnSelf()
    {
        if (pooledObject != null)
            pooledObject.Despawn();
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (mainCamera == null) return;


        Vector3 viewport = mainCamera.WorldToViewportPoint(transform.position);

        //remove if off screen
        if (viewport.x < -destroyMargin || viewport.x > 1f + destroyMargin || viewport.y < -destroyMargin || viewport.y > 1f + destroyMargin)
        {
            Die();
        }
    }

    public void OnPlayerHit(BasePlayerData player)
    {
        if (player != null && player.canTakeDamage)
        {
            player.TakeDamage(1);
        }

        Die();
    }

}
