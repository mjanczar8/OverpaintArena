using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public static ScreenBounds Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float extraMargin = 1.5f;

    public Vector2 Min {  get; private set; }
    public Vector2 Max { get; private set; }

    public float Width => Max.x - Min.x;
    public float Height => Max.y - Min.y;

    public float Left => Min.x;
    public float Right => Max.x;

    public float Bottom => Min.y;
    public float Top => Max.y;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if(mainCamera == null)
            mainCamera = Camera.main;

        CalculateBounds();
    }

    private void CalculateBounds()
    {
        float zDist = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);

        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, zDist));

        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, zDist));

        Min = new Vector2(bottomLeft.x - extraMargin, bottomLeft.y - extraMargin);

        Max = new Vector2(topRight.x + extraMargin, topRight.y + extraMargin);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        Gizmos.color = Color.green;

        Vector3 center = (Min + Max) * 0.5f;

        Vector3 size = new Vector3(Width, Height, 0);

        Gizmos.DrawWireCube(center, size);

    }
#endif
}
