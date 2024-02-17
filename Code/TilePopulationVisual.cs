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
        thisTilePopulationVisual.transform.position = thisZone.transform.position - new Vector3(0, (thisZone.GetZoneSizeModifier().y / 2) - 0.5f, 0);
        
    }
    void Start()
    {
        thisTilePopulationVisual.text = thisZone.GetNumberOfCitizens().ToString();
        TurnSystem.Instance.OnTurnChanged += TilePopulationVisual_OnTurnChanged;
    }

    void Update()
    {
        
    }

    public void UpdateTilePopText()
    {
        thisTilePopulationVisual.text = thisZone.GetNumberOfCitizens().ToString("F2");
        thisTilePopulationVisual.text
    }

    private void TilePopulationVisual_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTilePopText();
    }
}
