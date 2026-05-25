using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    public static ScreenBounds Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private float extraMargin = 0f;
    private void Awake()
    {

    }
}
