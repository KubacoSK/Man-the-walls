using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    private List<Zone> AlliedControlledZones;
    private float totalPopulation;

    private void Awake()
    {

        if (Instance != null)
        {
            Debug.LogError("There's more than one ResourceManager! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        AlliedControlledZones = ZoneManager.ReturnAlliedZones();
        Zone.ZoneControlChanged += Zone_ZoneControlChanged;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        foreach (Zone zone in AlliedControlledZones)
        {
            totalPopulation += zone.GetNumberOfCitizens();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Zone_ZoneControlChanged(object sender, EventArgs e)
    {
        // checks if zone is changed to allied or enemy and removes it from the list
        Zone zone = sender as Zone;
        if (zone.IsUnderAllycont() && !AlliedControlledZones.Contains(zone)) AlliedControlledZones.Add(zone);
        if (!zone.IsUnderAllycont() && AlliedControlledZones.Contains(zone)) AlliedControlledZones.Remove(zone);
    }

    public void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            // calcualtes total population based on controlled zones
            totalPopulation = 0;
            foreach (Zone zone in AlliedControlledZones)
            {
                zone.PopulationGrowth();
                totalPopulation += zone.GetNumberOfCitizens();
               
            }
        }
    }

    public float GetNumberOfTotalPopulation()
    {
        return totalPopulation;
    }
}