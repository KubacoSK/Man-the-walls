using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TilePopulationVisual : MonoBehaviour
{
    [SerializeField] private Zone thisZone;
    [SerializeField] private TextMeshProUGUI thisTilePopulationVisual;

    private void Awake()
    {
        thisTilePopulationVisual.transform.position = thisZone.transform.position - new Vector3(0, (thisZone.GetZoneSizeModifier().y / 2) - 0.3f, 0); 
        // moves each text element so it starts at the bottom of the zone
    }
    void Start()
    {
        
        TurnSystem.Instance.OnTurnChanged += TilePopulationVisual_OnTurnChanged;     // subscribes the event that changes turns so it dynamically increases pop
        thisTilePopulationVisual.text = thisZone.GetNumberOfCitizens().ToString();
        thisTilePopulationVisual.fontSize = 0.8f + (thisZone.GetNumberOfCitizens() / 50);
    }

    void Update()
    {
        
    }

    public void UpdateTilePopText()
    {
        thisTilePopulationVisual.text = thisZone.GetNumberOfCitizens().ToString("F2");     // shows the actual number of citizens
        thisTilePopulationVisual.fontSize = 0.8f + (thisZone.GetNumberOfCitizens() / 50);  // increases size of the text depending on the number of citizens
        
    }

    private void TilePopulationVisual_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTilePopText();
    }
}
