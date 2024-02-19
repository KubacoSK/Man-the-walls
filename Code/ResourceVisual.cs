using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourceVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI CoalCountText;
    [SerializeField] private TextMeshProUGUI RedCrysCountText;
    [SerializeField] private TextMeshProUGUI BlueCrysCountText;
    [SerializeField] private TextMeshProUGUI SteelCountText;

    [SerializeField] private TextMeshProUGUI CoalIncomeText;
    [SerializeField] private TextMeshProUGUI RedCIncomeText;
    [SerializeField] private TextMeshProUGUI BluCIncomeText;
    [SerializeField] private TextMeshProUGUI SteelIncomeText;

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
        UpdateResourceIncomeVisual();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateResourceCountVisual();
    }

    private void UpdateResourceCountVisual()
    {
        // shows the current number of resources
        CoalCountText.text = "" + ResourceManager.Instance.CoalCount;
        RedCrysCountText.text = "" + ResourceManager.Instance.RedCryCount;
        BlueCrysCountText.text = "" + ResourceManager.Instance.BlueCryCount;
        SteelCountText.text = "" + ResourceManager.Instance.SteelCount;
    }
    private void UpdateResourceIncomeVisual()
    {
        // shows the income of the resource
        CoalIncomeText.text = "+" + ResourceManager.Instance.CoalIncome;
        RedCIncomeText.text = "+" + ResourceManager.Instance.RedCIncome;
        BluCIncomeText.text = "+" + ResourceManager.Instance.BluCIncome;
        SteelIncomeText.text = "+" + ResourceManager.Instance.SteelIncome;

    }

    private void AddResources()
    {

    }
}
