using UnityEngine;
using UnityEngine.UI;

public class DividerGlow : MonoBehaviour
{
    public float pulseSpeed = 2f;
    public float minAlpha = 0.3f;
    public float maxAlpha = 1f;
    private Image image;
    private Color baseColor;

    void Awake()
    {
        image = GetComponent<Image>();
        baseColor = image.color;
    }

    void Update()
    {
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f);
        image.color = new Color(baseColor.r, baseColor.g, baseColor.b, alpha);
    }
}
