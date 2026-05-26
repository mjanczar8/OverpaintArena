using UnityEngine;

public class DrifterPlayerHitbox : MonoBehaviour
{
    private DrifterEnemy drifter;

    private void Awake()
    {
        drifter = GetComponentInParent<DrifterEnemy>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var player = other.GetComponent<PlayerData>();
        if (player == null) return;

        drifter.OnPlayerHit(player);
    }
}