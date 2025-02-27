using System;
using TMPro;
using UnityEngine;

public class TilePopulationVisual : MonoBehaviour
{
    [SerializeField] private Zone thisZone;
    [SerializeField] private TextMeshProUGUI thisTilePopulationVisual;

    private void Awake()
    {
        thisTilePopulationVisual.transform.position = thisZone.transform.position - new Vector3(0, (thisZone.GetZoneSizeModifier().y / 2) - 0.3f, 0);
        // posunie každý textový prvok tak, aby začínal na spodnej časti zóny
    }

    void Start()
    {
        Zone.ZoneControlChanged += TilePop_ZoneControlChanged;
        TurnSystem.Instance.OnTurnChanged += TilePopulationVisual_OnTurnChanged;
        // odoberie sa na udalosť, ktorá mení ťahy, aby sa populácia dynamicky zvyšovala
        thisTilePopulationVisual.text = thisZone.GetNumberOfCitizens().ToString();
        thisTilePopulationVisual.fontSize = 0.8f + (thisZone.GetNumberOfCitizens() / 50);
    }

    void Update()
    {

    }

    public void UpdateTilePopText()
    {
        thisTilePopulationVisual.text = thisZone.GetNumberOfCitizens().ToString("F2");
        // zobrazí skutočný počet obyvateľov
        thisTilePopulationVisual.fontSize = 0.8f + (thisZone.GetNumberOfCitizens() / 50);
        // zväčší veľkosť textu v závislosti od počtu obyvateľov
    }

    private void TilePopulationVisual_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTilePopText();
    }

    public void TilePop_ZoneControlChanged(object sender, EventArgs e)
    {
        UpdateTilePopText();
    }
}
