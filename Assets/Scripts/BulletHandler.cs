using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BulletHandler : MonoBehaviour
{
	[Header("Damage")]
	public float damage = 1f;

	[Header("Cleanup")]
	public float margin = 0.08f;
	public float lifetime = 5f;
	public bool debugLines = true;

	private Camera mainCamera;
	private PooledObject pooledObject;
	private Rigidbody2D rb;
	private float lifetimeTimer;

	private void Awake()
	{
		if (mainCamera == null)
			mainCamera = Camera.main;

		pooledObject = GetComponent<PooledObject>();
		rb = GetComponent<Rigidbody2D>();

		lifetimeTimer = lifetime;
	}

	private void OnEnable()
	{
		lifetimeTimer = lifetime;

		if (rb != null)
		{
			rb.linearVelocity = Vector2.zero;
			rb.angularVelocity = 0f;
		}
	}

	private void Update()
	{
		if (mainCamera == null)
			return;

		//lifetime cleanup
		if (lifetime > 0f)
		{
			lifetimeTimer -= Time.deltaTime;
			if (lifetimeTimer <= 0f)
			{
				Despawn();
				return;
			}
		}

		//off-screen cleanup
		Vector2 viewport = mainCamera.WorldToViewportPoint(transform.position);

		if (debugLines)
			DrawDebugLines();

		if (viewport.x < -margin || viewport.x > 1f + margin ||
			viewport.y < -margin || viewport.y > 1f + margin)
		{
			Despawn();
		}
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        var enemy = other.GetComponent<DrifterEnemy>();
        if (enemy == null)
            return;

        enemy.TakeDamage(damage);
        Despawn();
    }

    private void Despawn()
	{
		if (pooledObject != null)
			pooledObject.Despawn();
		else
			Destroy(gameObject);
	}

	private void DrawDebugLines()
	{
		Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f - margin, 0f - margin, mainCamera.nearClipPlane));
		Vector3 bottomRight = mainCamera.ViewportToWorldPoint(new Vector3(1f + margin, 0f - margin, mainCamera.nearClipPlane));
		Vector3 topLeft = mainCamera.ViewportToWorldPoint(new Vector3(0f - margin, 1f + margin, mainCamera.nearClipPlane));
		Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1f + margin, 1f + margin, mainCamera.nearClipPlane));

		Debug.DrawLine(bottomLeft, bottomRight, Color.red);
		Debug.DrawLine(bottomRight, topRight, Color.red);
		Debug.DrawLine(topRight, topLeft, Color.red);
		Debug.DrawLine(topLeft, bottomLeft, Color.red);
	}
}
