using UnityEngine;


public class PlayerPainter : MonoBehaviour
{
    [SerializeField] private PaintMap paintMap;

    [SerializeField] private Transform leftPaintPoint;
    [SerializeField] private Transform rightPaintPoint;

    [SerializeField] private int thickness = 2;

    void Update()
    {
        paintMap.PaintEdgeLine(
            leftPaintPoint.position,
            rightPaintPoint.position,
            thickness,
            1
        );
    }
}