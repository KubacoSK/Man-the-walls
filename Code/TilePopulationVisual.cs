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
        
    }
    void Start()
    {
        
        TurnSystem.Instance.OnTurnChanged += TilePopulationVisual_OnTurnChanged;
        thisTilePopulationVisual.text = thisZone.GetNumberOfCitizens().ToString();
        thisTilePopulationVisual.fontSize = 0.8f + (thisZone.GetNumberOfCitizens() / 50);
    }

    void Update()
    {
        
    }

    public void UpdateTilePopText()
    {
        thisTilePopulationVisual.text = thisZone.GetNumberOfCitizens().ToString("F2");
        thisTilePopulationVisual.fontSize = 0.8f + (thisZone.GetNumberOfCitizens() / 50);
        
    }

    private void TilePopulationVisual_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTilePopText();
    }
}
