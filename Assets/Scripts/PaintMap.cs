using UnityEngine;

public class PaintMap : MonoBehaviour
{
    [Header("Arena")]
    [SerializeField] private Vector2 arenaWorldSize = new Vector2(18f, 10f);

    [Header("Paint Map")]
    [SerializeField] private int paintWidth = 512;//256
    [SerializeField] private int paintHeight = 288;//144

    [Header("Colors")]
    [SerializeField] private Color32 neutralColor = new Color32(20, 20, 20, 255);
    [SerializeField] private Color32 playerColor = new Color32(20, 20, 20, 255);
    [SerializeField] private Color32 enemyColor = new Color32(255, 40, 120, 255);

    private Texture2D paintTexture;
    private Color32[] pixels;
    private byte[] ownership;

    private SpriteRenderer rend;

    private void Awake()
    {
        rend = GetComponent<SpriteRenderer>();

        paintTexture = new Texture2D(paintWidth, paintHeight, TextureFormat.RGBA32, false);
        paintTexture.filterMode = FilterMode.Point;
        paintTexture.wrapMode = TextureWrapMode.Clamp;

        pixels = new Color32[paintWidth * paintHeight];
        ownership = new byte[paintWidth * paintHeight];

        for (int i = 0; i < pixels.Length; i++)
        {
            pixels[i] = neutralColor;
            ownership[i] = 0;
        }

        paintTexture.SetPixels32(pixels);
        paintTexture.Apply();

        Sprite paintSprite = Sprite.Create(
            paintTexture,
            new Rect(0, 0, paintWidth, paintHeight),
            new Vector2(0.5f, 0.5f),
            paintWidth / arenaWorldSize.x
        );

        paintSprite.name = "Runtime Paint Map Sprite";

        rend.sprite = paintSprite;
        rend.color = Color.white;

        Debug.Log("Sprite assigned: " + rend.sprite.name);
        Debug.Log("Texture pixel 0: " + paintTexture.GetPixel(0, 0));
    }

    public void PaintCircle(Vector2 worldPos, int radius, byte owner)
    {
        Vector2Int center = WorldToPaint(worldPos);

        for (int y = -radius; y <= radius; y++)
        {
            for (int x = -radius; x <= radius; x++)
            {
                if (x * x + y * y > radius * radius)
                    continue;

                int px = center.x + x;
                int py = center.y + y;

                if (px < 0 || px >= paintWidth || py < 0 || py >= paintHeight)
                    continue;

                int index = py * paintWidth + px;

                ownership[index] = owner;

                if (owner == 1)
                    pixels[index] = playerColor;
                else if (owner == 2)
                    pixels[index] = enemyColor;
                else
                    pixels[index] = neutralColor;
            }
        }

        paintTexture.SetPixels32(pixels);
        paintTexture.Apply();
    }

    public void PaintEdgeLine(Vector2 leftWorld, Vector2 rightWorld, int thickness, byte owner)
    {
        float distance = Vector2.Distance(leftWorld, rightWorld);

        float stepSize = arenaWorldSize.x / paintWidth;

        int steps = Mathf.CeilToInt(distance / stepSize);

        for (int i = 0; i <= steps; i++)
        {
            float t = steps == 0 ? 0 : (float)i / steps;

            Vector2 point = Vector2.Lerp(
                leftWorld,
                rightWorld,
                t);

            PaintCircle(point, thickness, owner);
        }
    }



    private Vector2Int WorldToPaint(Vector2 worldPos)
    {
        Vector2 bottomLeft = (Vector2)transform.position - arenaWorldSize * 0.5f;
        Vector2 local = worldPos - bottomLeft;

        int x = Mathf.FloorToInt(local.x / arenaWorldSize.x * paintWidth);
        int y = Mathf.FloorToInt(local.y / arenaWorldSize.y * paintHeight);

        return new Vector2Int(x, y);
    }
}