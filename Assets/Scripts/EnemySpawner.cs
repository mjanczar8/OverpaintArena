using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;
    public GameObject drifterPrefab;
    public MasterPool pool;
    public Camera mainCamera;

    [Header("Spawn Settings")]
    public float spawnInterval = 1.5f;
    public int maxDriftEnemies = 8;
    public float minDistanceFromPlayer = 3f;
    private float viewportMargin = 0.06f;

    [Header("Initial Motion")]
    public Vector2 initialSpeedRange = new Vector2(1f, 3f);
    public Vector2 torqueRange = new Vector2(-5f, 5f);
    public float aimNoiseDegrees = 20f;

    private int alive;

    private void Awake()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    void OnEnable() => StartCoroutine(SpawnLoop());
    void OnDisable() => StopAllCoroutines();

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (alive < maxDriftEnemies) SpawnOne();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    void SpawnOne()
    {
        if (drifterPrefab == null || mainCamera == null) return;

        Vector2 vpPos = Vector2.zero;
        int edge = Random.Range(0, 4);
        switch (edge)
        {
            case 0: vpPos = new Vector2(-viewportMargin, Random.value); break;
            case 1: vpPos = new Vector2(1f + viewportMargin, Random.value); break;
            case 2: vpPos = new Vector2(Random.value, -viewportMargin); break;
            case 3: vpPos = new Vector2(Random.value, 1f + viewportMargin); break;
        }

        float z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 worldPos = mainCamera.ViewportToWorldPoint(new Vector3(vpPos.x, vpPos.y, z));

        //avoid spawning on player
        if (player != null && Vector2.Distance(player.position, worldPos) < minDistanceFromPlayer)
            return;

        Quaternion rotation = Quaternion.Euler(0, 0, Random.Range(0f, 360f));
        GameObject obj = pool.Spawn(drifterPrefab, worldPos, rotation);

        var drifter = obj.GetComponent<DriftEnemy>();
        if (drifter != null)
        {
            drifter.SetSpawner(this);
        }

        //initial push toward screen center / player
        var rb = obj.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Vector2 dir;
            if (player != null)
                dir = ((Vector2)player.position - (Vector2)worldPos).normalized;
            else
            {
                //bias toward camera center
                Vector2 center = mainCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, z));
                dir = (center - (Vector2)worldPos).normalized;
            }

            float noise = Random.Range(-aimNoiseDegrees, aimNoiseDegrees);
            dir = Quaternion.Euler(0, 0, noise) * dir;

            float speed = Random.Range(initialSpeedRange.x, initialSpeedRange.y);
            rb.linearVelocity = dir * speed;
            rb.angularVelocity = Random.Range(torqueRange.x, torqueRange.y);
        }
        alive++;
    }

    public void NotifyAsteroidDestroyed()
    {
        alive = Mathf.Max(0, alive - 1);
    }
}
