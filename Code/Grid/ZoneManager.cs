using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance { get; private set; }

    [SerializeField] private List<Zone> zones = new List<Zone>();
    private List<Zone> AlliedZones;
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

    private void Start()
    {
        Zone.ZoneControlChanged += Zone_ZoneControlChanged;
    }

    public void AddUnitToZone(Unit unit, Zone zone)
    {
        zone.AddUnit(unit);
    }

    public void RemoveUnitFromZone(Unit unit, Zone zone)
    {
        zone.RemoveUnit(unit);
    }
    public static List<Zone> GetAllZones()
    {
        return Instance.zones;
    }

    private void Zone_ZoneControlChanged(object sender, EventArgs e)
    {
        Zone zone = sender as Zone;
        if (zone.IsUnderAllycont() && !AlliedZones.Contains(zone))
        {
            AlliedZones.Add(zone);
        }
        else if(!zone.IsUnderAllycont() && AlliedZones.Contains(zone))
        {
            AlliedZones.Remove(zone);
        }
    }
}