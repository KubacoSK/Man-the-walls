using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    private List<Unit> unitsInZone = new List<Unit>();

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is a unit
        if (other.CompareTag("Unit"))
        {
            // if there is any unit with collider inside object it adds it to list
            Unit unit = other.GetComponent<Unit>();
            ZoneManager.Instance.AddUnitToZone(unit, this);
            unitsInZone.Add(unit);

            Debug.Log(unit.name + " entered " + name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is a unit
        if (other.CompareTag("Unit"))
        {
            Unit unit = other.GetComponent<Unit>();
            ZoneManager.Instance.RemoveUnitFromZone(unit, this);
            unitsInZone.Remove(unit);

            Debug.Log(unit.name + " exited " + name);
        }
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


}
