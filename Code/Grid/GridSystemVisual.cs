using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Color highlightColor = Color.blue;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
    }

    public void Highlight()
    {
        spriteRenderer.color = highlightColor;
    }

    public void ResetHighlight()
    {
        spriteRenderer.color = originalColor;
    }

}
