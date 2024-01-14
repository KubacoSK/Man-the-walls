using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;
    private List<Zone> AlliedControlledZones;
    private int NumberOfCitizens;

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

        AlliedControlledZones = new List<Zone>();
        Zone.ZoneControlChanged += Zone_ZoneControlChanged;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Zone_ZoneControlChanged(object sender, EventArgs e)
    {
        AlliedControlledZones.Clear();
        AlliedControlledZones = ZoneManager.ReturnAlliedZones();
        NumberOfCitizens = 0;
        foreach (Zone zone in AlliedControlledZones)
        {
            NumberOfCitizens += zone.GetNumberOfCitizens();
        }
    }
}