using System;

using TMPro;
using UnityEngine;

public class ResourceVisual : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI populationCountText;

    [SerializeField] private TextMeshProUGUI CoalCountText;
    [SerializeField] private TextMeshProUGUI RedCrysCountText;
    [SerializeField] private TextMeshProUGUI BlueCrysCountText;
    [SerializeField] private TextMeshProUGUI SteelCountText;

    [SerializeField] private TextMeshProUGUI CoalIncomeText;
    [SerializeField] private TextMeshProUGUI RedCIncomeText;
    [SerializeField] private TextMeshProUGUI BluCIncomeText;
    [SerializeField] private TextMeshProUGUI SteelIncomeText;

    public static ResourceVisual Instance { get; private set; }
    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There's more than one ResourceVisual! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void Start()
    {
        Zone.ZoneControlChanged += Zone_ZoneControlChanged;
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        populationCountText.text = "Population: " + Math.Floor(ResourceManager.Instance.GetNumberOfTotalPopulation() * 1000);
        
    }

    void Update()
    {
        
    }

    private void Zone_ZoneControlChanged(object sender, EventArgs e)
    {
        UpdateResourceIncomeVisual();
        populationCountText.text = "Population: " + Math.Floor(ResourceManager.Instance.GetNumberOfTotalPopulation() * 1000);   // uistíme sa že nám zóna ukáže správny počet populácie
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        UpdateResourceCountVisual();
        populationCountText.text = "Population: " + Math.Floor(ResourceManager.Instance.GetNumberOfTotalPopulation() * 1000);
    }

    public void UpdateResourceCountVisual()
    {
        // ukazuje počet surovín
        CoalCountText.text = "" + ResourceManager.Instance.CoalCount;
        RedCrysCountText.text = "" + ResourceManager.Instance.RedCryCount;
        BlueCrysCountText.text = "" + ResourceManager.Instance.BlueCryCount;
        SteelCountText.text = "" + ResourceManager.Instance.SteelCount;
    }
    public void UpdateResourceIncomeVisual()
    {
        // ukazuje príjem surovín
        CoalIncomeText.text = "+" + ResourceManager.Instance.CoalIncome;
        RedCIncomeText.text = "+" + ResourceManager.Instance.RedCIncome;
        BluCIncomeText.text = "+" + ResourceManager.Instance.BluCIncome;
        SteelIncomeText.text = "+" + ResourceManager.Instance.SteelIncome;

    }
}
