using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    private List<Unit> unitsInZone;
    private GridSystemVisual highlighter;
    private bool IsUnderAllyControl;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private float NumberOfCitizens = 1;
    [SerializeField] private Color neutralColor;
    [SerializeField] private Color enemyColor;
    [SerializeField] private Color AllyColor;
    private Color CurrentColor;

    [SerializeField] private bool IsWall = false;

    public static event EventHandler ZoneControlChanged;

    private void Start()
    {
        highlighter = GetComponent<GridSystemVisual>();
        unitsInZone = new List<Unit>();
        IsUnderAllyControl = true;
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentColor = AllyColor;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is a unit
        if (other.CompareTag("Unit") || other.CompareTag("EnemyUnit"))
        {
            // If there is any unit with collider inside the object, add it to the list
            Unit unit = other.GetComponent<Unit>();

            if (unit != null && !unitsInZone.Contains(unit))
            {
                ZoneManager.Instance.AddUnitToZone(unit, this);

            }
        }

    }
    public void InitiateEliminationProcess(Zone zone)
    {
        // using this to get units in zone list
        UnitCombat.Instance.TryEliminateUnits(unitsInZone, zone);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is a unit
        if (other.CompareTag("Unit") || other.CompareTag("EnemyUnit"))
        {
            Unit unit = other.GetComponent<Unit>();
            ZoneManager.Instance.RemoveUnitFromZone(unit, this);

        }
    }

    public Vector2 GetZoneSizeModifier()
    {
        Collider2D collider = GetComponent<Collider2D>();

        // Getting the size from the collider's bounds
        Vector2 size = collider.bounds.size;
        return size;

    }

    public void AddUnit(Unit unit)
    {
        unitsInZone.Add(unit);
        // Additional actions you want to perform when a unit enters the zone
    }

    public void RemoveUnit(Unit unit)
    {
        unitsInZone.Remove(unit);
        // Additional actions you want to perform when a unit exits the zone
    }
    public Zone GetClickedZone(Vector3 mouseWorldPosition)
    {
        // checks if you click on an object with collider and if it has zone component
        Collider2D collider = Physics2D.OverlapPoint(mouseWorldPosition, LayerMask.GetMask("GridPoints"));

        if (collider != null)
        {
            return collider.GetComponent<Zone>();
        }

        return null;
    }

    public void Highlight()
    {
        if (highlighter != null)
        {
            highlighter.Highlight(this);
        }
    }

    public void ResetHighlight()
    {
        if (highlighter != null)
        {
            // no idea why this is like it
            highlighter.ResetHighlight(this);
        }
    }
    public List<Unit> GetUnitsInZone()
    {
        return unitsInZone;
    }
    public bool IsWallCheck()
    {
        return IsWall;
    }
    public List<Unit> ReturnAllyUnitsInZone()
    {
        // returns a list of all allied units
        List<Unit> AllyUnits = new List<Unit>();
        foreach (Unit unit in unitsInZone)
        {
            if (!unit.IsEnemy()) AllyUnits.Add(unit);
        }
        return AllyUnits;
    }
    public List<Unit> ReturnEnemyUnitsInZone()
    {
        // returns a list of all allied units
        List<Unit> EnemyUnits = new List<Unit>();
        foreach (Unit unit in unitsInZone)
        {
            if (unit.IsEnemy()) EnemyUnits.Add(unit);
        }
        return EnemyUnits;
    }

    public bool IsUnderAllycont()
    {
        return IsUnderAllyControl;
    }

    public Color ReturnCurrentColor()
    {
        return CurrentColor;
    }

    public void ChangeControlToAlly()
    {
        IsUnderAllyControl = true;
        CurrentColor = AllyColor;
        spriteRenderer.color = CurrentColor;
        ZoneControlChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeControlToEnemy()
    {
        IsUnderAllyControl = false;
        CurrentColor = enemyColor;
        spriteRenderer.color = CurrentColor;
        ZoneControlChanged?.Invoke(this, EventArgs.Empty);
    }

    public void ChangeControlToNeutral()
    {
        IsUnderAllyControl = false;
        CurrentColor = neutralColor;
        spriteRenderer.color = CurrentColor;
        ZoneControlChanged?.Invoke(this, EventArgs.Empty);
    }

    public float GetNumberOfCitizens()
    {
        return NumberOfCitizens;
    }

    public void PopulationGrowth()
    {
        NumberOfCitizens = NumberOfCitizens * 1.15f + 0.2f;
    }
}