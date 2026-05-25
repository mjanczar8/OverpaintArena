using UnityEngine;

public class PooledObject : MonoBehaviour
{
	[HideInInspector] public MasterPool pool;
	[HideInInspector] public GameObject prefab;

	public void Despawn()
	{
		if (pool != null && prefab != null)
			pool.Despawn(prefab, gameObject);
		else
			Destroy(gameObject);
	}
}
