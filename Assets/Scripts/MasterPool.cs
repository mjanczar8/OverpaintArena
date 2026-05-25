using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class MasterPool : MonoBehaviour
{
	[System.Serializable]
	public class PoolEntry
	{
		public GameObject prefab;
		public int initialSize = 10;
	}

	[Header("Pool Setup")]
	[Tooltip("Prefab (Prefab) : NEEDS PooledObject script. \nInitial Size (Int) : Starting pool size.")]
	public PoolEntry[] entries;

	private readonly Dictionary<GameObject, Queue<GameObject>> pools = new();

	void Awake()
	{
		foreach (var entry in entries)
		{
			if (entry.prefab == null) continue;

			var queue = new Queue<GameObject>();
			pools[entry.prefab] = queue;

			for (int i = 0; i < entry.initialSize; i++)
			{
				GameObject obj = Instantiate(entry.prefab, transform);
				obj.SetActive(false);

				var poolObj = obj.GetComponent<PooledObject>();
				if (poolObj != null)
				{
					poolObj.pool = this;
					poolObj.prefab = entry.prefab;
				}
				else
				{
					Debug.LogWarning($"[MasterPool] Prefab '{entry.prefab.name}' has no PooledObject script!", entry.prefab);
				}

				queue.Enqueue(obj);
			}
		}
	}

	public GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
	{
		if (prefab == null)
			return null;

		if (!pools.TryGetValue(prefab, out var queue))
		{
			queue = new Queue<GameObject>();
			pools[prefab] = queue;
		}

		GameObject obj;
		if (queue.Count > 0)
		{
			obj = queue.Dequeue();
		}
		else
		{
			obj = Instantiate(prefab, transform);
		}

		obj.transform.SetPositionAndRotation(position, rotation);
		obj.SetActive(true);

		var poolObj = obj.GetComponent<PooledObject>();
		if (poolObj != null)
		{
			poolObj.pool = this;
			poolObj.prefab = prefab;
		}
		else
		{
			Debug.LogWarning($"[MasterPool] Spawned object from prefab '{prefab.name}' has no PooledObject script!", prefab);
		}

		return obj;
	}

	public void Despawn(GameObject prefab, GameObject instance)
	{
		if (instance == null) return;

		instance.SetActive(false);

		if (!pools.TryGetValue(prefab, out var queue))
		{
			queue = new Queue<GameObject>();
			pools[prefab] = queue;
		}

		queue.Enqueue(instance);
	}
}
