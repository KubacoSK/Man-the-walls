using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance { get; private set; }

    [SerializeField] private List<Zone> zones;
    private static List<Zone> AlliedZones;  // Removed the assignment here

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

        AlliedZones = new List<Zone>(zones);  // Initialize the list here
    }

    private void Start()
    {
        Zone.ZoneControlChanged += Zone_ZoneControlChanged;
        // Remove the line that assigns AlliedZones here
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
        else if (!zone.IsUnderAllycont() && AlliedZones.Contains(zone))
        {
            AlliedZones.Remove(zone);
        }
    }

    public static List<Zone> ReturnAlliedZones()
    {
        return AlliedZones;
    }
}