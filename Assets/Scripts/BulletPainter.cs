using UnityEngine;

public class BulletPainter : MonoBehaviour
{
    [SerializeField] private PaintMap paintMap;

    [SerializeField] private Transform leftPaintPoint;
    [SerializeField] private Transform rightPaintPoint;

    [SerializeField] private int thickness = 1;

    private void Awake()
    {
        if (paintMap == null)
            paintMap = PaintMap.Instance;
    }

    void Update()
    {
        if (paintMap == null)
            return;

        paintMap.PaintEdgeLine(leftPaintPoint.position, rightPaintPoint.position, thickness, 1);
    }

}
