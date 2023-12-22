using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    private List<Unit> unitsInZone = new List<Unit>();
    private GridSystemVisual highlighter;

    private void Start()
    {
        highlighter = GetComponent<GridSystemVisual>();
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is a unit
        if (other.CompareTag("Unit") || other.CompareTag("EnemyUnit"))
        {
            // if there is any unit with collider inside object it adds it to list
            Unit unit = other.GetComponent<Unit>();
            ZoneManager.Instance.AddUnitToZone(unit, this);
            unitsInZone.Add(unit);
        }
        
    }
    public void InitiateEliminationProcess()
    {
        UnitCombat.Instance.TryEliminateUnits(unitsInZone);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is a unit
        if (other.CompareTag("Unit") || other.CompareTag("EnemyUnit"))
        {
            Unit unit = other.GetComponent<Unit>();
            ZoneManager.Instance.RemoveUnitFromZone(unit, this);
            unitsInZone.Remove(unit);
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
            highlighter.Highlight();
        }
    }

    public void ResetHighlight()
    {
        if (highlighter != null)
        {
            highlighter.ResetHighlight();
        }
    }
    

}
