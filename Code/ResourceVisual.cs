using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CoalCount;
    [SerializeField] private TextMeshProUGUI RedCrysCount;
    [SerializeField] private TextMeshProUGUI BlueCrysCount;

    [SerializeField] private TextMeshProUGUI CoalIncomeText;
    [SerializeField] private TextMeshProUGUI RedCIncomeText;
    [SerializeField] private TextMeshProUGUI BluCIncomeText;
    void Start()
    {
        Zone.ZoneControlChanged += Zone_ZoneControlChanged;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    void Update()
    {
        
    }

    private void Zone_ZoneControlChanged(object sender, EventArgs e)
    {
        
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {

    }

    private void UpdateResources(Zone zone)
    {
    }

    private void AddResources()
    {

    }
}
