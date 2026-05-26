using UnityEngine;

public class EnemyPainter : MonoBehaviour
{
    [SerializeField] private PaintMap paintMap;

    [SerializeField] private int brushRadius = 4;

    private void Awake()
    {
        TryGrabPaintMap();
    }

    private void OnEnable()
    {
        TryGrabPaintMap();
    }

    void Update()
    {
        TryGrabPaintMap();

        paintMap.PaintCircle(transform.position, brushRadius, 2);
    }

    private bool TryGrabPaintMap()
    {
        if (paintMap != null)
            return true;

        paintMap = PaintMap.Instance;

        return paintMap != null;
    }
}
