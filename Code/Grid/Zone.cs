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
        TryEliminateUnits();
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
    private void TryEliminateUnits()
    {
        // Check if there are at least two units in the zone
        if (unitsInZone.Count >= 2)
        {
            // Filter units by type (e.g., ally and enemy)
            List<Unit> allyUnits = new List<Unit>();
            List<Unit> enemyUnits = new List<Unit>();

            foreach (Unit unit in unitsInZone)
            {
                if (unit.tag == "Unit")  // You should have a method to determine if the unit is an ally or enemy
                {
                    allyUnits.Add(unit);
                }
                else
                {
                    enemyUnits.Add(unit);
                }
            }

            // If there is at least one ally and one enemy, randomly eliminate one of them
            if (allyUnits.Count > 0 && enemyUnits.Count > 0)
            {
                Unit unitToEliminate = Random.Range(0, 2) == 0 ? allyUnits[0] : enemyUnits[0];

                // Perform elimination logic (e.g., destroy the unit)
                StartCoroutine(DelayedElimination(unitToEliminate));
            }
        }
    }

    private IEnumerator DelayedElimination(Unit unit)
    {
        // Wait for 2 seconds
        yield return new WaitForSeconds(2f);

        // Perform elimination logic (e.g., destroy the unit)
        EliminateUnit(unit);
    }
    private void EliminateUnit(Unit unit)
    {
        // Additional logic for eliminating the unit
        Debug.Log("Eliminating unit: " + unit.name);
        Destroy(unit.gameObject);
    }

}
