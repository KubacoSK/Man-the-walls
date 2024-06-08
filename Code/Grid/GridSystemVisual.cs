using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    private Color highlightColor = Color.blue;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Highlight(Zone zone)
    {
        highlightColor = zone.ReturnCurrentColor();   // we highlight zone with bit of transparency
        highlightColor.a = zone.ReturnCurrentColor().a + 0.25f;
        spriteRenderer.color = highlightColor;
    }

    public void ResetHighlight(Zone zone)
    {
        spriteRenderer.color = zone.ReturnCurrentColor();
    }

}
