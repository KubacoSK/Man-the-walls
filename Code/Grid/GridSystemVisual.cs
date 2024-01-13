using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    private Color highlightColor = Color.blue;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        foreach (Zone zone in ZoneManager.GetAllZones())
        {
            if (zone.ReturnEnemyUnitsInZone().Count > 0)
            {
                zone.ChangeControlToEnemy();
            }
        }
    }

    public void Highlight(Zone zone)
    {
        highlightColor = zone.ReturnCurrentColor();
        highlightColor.a = 0.45f;
        spriteRenderer.color = highlightColor;
    }

    public void ResetHighlight(Zone zone)
    {
        spriteRenderer.color = zone.ReturnCurrentColor();
    }

}
