using TMPro;
using System.Collections;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI hpTxt;

    [Header("Damage / I-Frames")]
    public bool canTakeDamage = true;
    private int health = 10;
    private float iFrameDuration = 0.3f;
    private float iFrameTimer = 0f;

    [Header("Damage Flash")]
    public SpriteRenderer spriteRenderer;
    public float flashDuration = 0.3f;
    public Color flashColor = Color.red;

    private Color originalColor;

    private void Awake()
    {
        if (canvas == null)
            canvas = FindAnyObjectByType<Canvas>();

        if (hpTxt == null)
            hpTxt = canvas.GetComponentInChildren<TextMeshProUGUI>(true);

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        originalColor = spriteRenderer.color;

        UpdateHpUI();
    }

    private void Update()
    {
        if (!canTakeDamage)
        {
            iFrameTimer -= Time.deltaTime;
            if (iFrameTimer <= 0f)
            {
                canTakeDamage = true;
            }
        }
    }

    public void TakeDamage(int amount)
    {
        if (!canTakeDamage) return;

        health -= amount;
        UpdateHpUI();

        canTakeDamage = false;
        iFrameTimer = iFrameDuration;

        StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash()
    {
        spriteRenderer.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }

    private void UpdateHpUI()
    {
        if (hpTxt != null)
            hpTxt.text = $"HP: {health}";
    }
}
