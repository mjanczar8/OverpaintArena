using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
	[Header("Bullet")]
	public GameObject bulletPrefab;
	public float bulletSpeed = 20f;
	public float damage = 1f;

	[Header("Fire Settings")]
	public float fireRate = 0.25f;
	private float nextFireTime = 0f;

	[Header("Pooling")]
	public MasterPool pool;

	private Transform firePoint;

	private void Awake()
	{
		firePoint = transform;
	}

	public void TryShoot()
	{
		if (Time.time < nextFireTime) return;
		nextFireTime = Time.time + fireRate;

		if (bulletPrefab == null || pool == null) return;


		GameObject bullet = pool.Spawn(bulletPrefab, firePoint.position, firePoint.rotation);

		var rb = bullet.GetComponent<Rigidbody2D>();
		if (rb != null)
			rb.linearVelocity = firePoint.up * bulletSpeed;

		var bulletHandler = bullet.GetComponent<BulletHandler>();
		if (bulletHandler != null)
			bulletHandler.damage = damage;
	}
}
