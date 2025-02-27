using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    private Color highlightColor = Color.magenta;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Highlight(Zone zone)
    {
        highlightColor = zone.ReturnCurrentColor();   // zvyraznime zonu ale aby bola trochu priesvitna
        highlightColor.a = zone.ReturnCurrentColor().a + 0.25f;
        spriteRenderer.color = highlightColor;
    }

    public void ResetHighlight(Zone zone)
    {
        spriteRenderer.color = zone.ReturnCurrentColor();
    }

}
