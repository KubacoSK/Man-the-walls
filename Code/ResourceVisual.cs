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
        UpdateResourceIncomeVisual();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateResourceCountVisual();
    }

    private void UpdateResourceCountVisual()
    {
        CoalCountText.text = "" + ResourceManager.Instance.CoalCount;
        RedCrysCountText.text = "" + ResourceManager.Instance.RedCryCount;
        BlueCrysCountText.text = "" + ResourceManager.Instance.BlueCryCount;
    }
    private void UpdateResourceIncomeVisual()
    {
        CoalIncomeText.text = "+" + ResourceManager.Instance.CoalIncome;
        RedCIncomeText.text = "+" + ResourceManager.Instance.RedCIncome;
        BluCIncomeText.text = "+" + ResourceManager.Instance.BluIncome;
    }

    private void AddResources()
    {

    }
}
