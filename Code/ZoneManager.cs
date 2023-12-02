using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance { get; private set; }

    [SerializeField] private List<Zone> zones = new List<Zone>();
    // has list of all zones and manages if units enter and exit them

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one ZoneManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddUnitToZone(Unit unit, Zone zone)
    {
        zone.AddUnit(unit);
    }

    public void RemoveUnitFromZone(Unit unit, Zone zone)
    {
        zone.RemoveUnit(unit);
    }
}