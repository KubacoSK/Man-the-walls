using System;
using System.Collections.Generic;
using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance { get; private set; }

    [SerializeField] private List<Zone> zones;
    private static List<Zone> AlliedZones;  // Odstránená inicializácia tu

    // obsahuje zoznam všetkých zón a spravuje vstup a výstup jednotiek

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existuje viac ako jeden ZoneManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;

        AlliedZones = new List<Zone>(zones);  // Inicializácia zoznamu tu
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
        Zone zone = sender as Zone; // tento skript aktualizuje zoznam zón vždy, keď niekto obsadí zónu
        if (zone.WhoIsUnderControl() == Zone.ControlType.allied && !AlliedZones.Contains(zone))
        {
            AlliedZones.Add(zone);
        }
        else if ((zone.WhoIsUnderControl() == Zone.ControlType.enemy || zone.WhoIsUnderControl() == Zone.ControlType.neutral) && AlliedZones.Contains(zone))
        {
            AlliedZones.Remove(zone);
        }
    }

    public List<Zone> ReturnAlliedZones()
    {
        return AlliedZones;
    }
}
