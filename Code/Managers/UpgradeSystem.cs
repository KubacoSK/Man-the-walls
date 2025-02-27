
using TMPro;
using UnityEngine;
using static Unit;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;
    private bool hasUpgradedCitizensIncrease;
    private bool hasUpgradedInfantryStrength;
    private bool hasUpgradedHorsemanStrength;
    private bool hasUpgradedWalls;
    private bool hasUpgradedWallsLevel2;
    private bool hasIncreasedCoalIncome;
    private bool hasIncreasedSteelIncome;
    private bool hasIncreasedSoldierRecruitment;
    private bool hasIncreasedSoldierTrainedLimit;

    [SerializeField] private GameObject WallLevel1Button;
    [SerializeField] private GameObject WallLevel2Button;
    [SerializeField] private GameObject WallsLvl2Visual;
    [SerializeField] private GameObject WallsLvl3Visual;
    [SerializeField] private TextMeshProUGUI CitizensIncreaseText;
    [SerializeField] private TextMeshProUGUI InfantryStrengthText;
    [SerializeField] private TextMeshProUGUI HorsemanStrengthText;
    [SerializeField] private TextMeshProUGUI UpgradedWallsText;
    [SerializeField] private TextMeshProUGUI UpgradedWallsLevel2Text;
    [SerializeField] private TextMeshProUGUI CoalIncomeText;
    [SerializeField] private TextMeshProUGUI SteelIncomeText;
    [SerializeField] private TextMeshProUGUI SoldierRecruitmentText;
    [SerializeField] private TextMeshProUGUI SoldierMaxTrainingText;
    

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
    public void ActivateCitizensIncrease()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(6, 2, 2, 10) && !hasUpgradedCitizensIncrease)
        {
            // zvýši rast populácie
            ResourceManager.Instance.SteelCount -= 6;
            ResourceManager.Instance.BlueCryCount -= 2;
            ResourceManager.Instance.RedCryCount -= 2;
            ResourceManager.Instance.CoalCount -= 10;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            Zone.numberPopGrowth = 0.1f;
            Zone.percentagePopGrowth = 1.08f;
            hasUpgradedCitizensIncrease = true;
            CitizensIncreaseText.fontStyle = FontStyles.Bold;
            CitizensIncreaseText.text = "UPGRADED";
        }
    }
    public void IncreaseInfantryStrength()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(10, 15, 15, 0) && !hasUpgradedInfantryStrength)
        {
            // pridá jednu silu pechote
            ResourceManager.Instance.SteelCount -= 10;
            ResourceManager.Instance.BlueCryCount -= 15;
            ResourceManager.Instance.RedCryCount -= 15;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            foreach (Unit unit in UnitManager.Instance.GetFriendlyUnitList()) 
                if (hasIncreasedStrength && unit.TypeOfUnit == UnitType.Infantry) 
                    unit.IncreaseStrength();
            Unit.hasIncreasedStrength = true;
            hasUpgradedInfantryStrength = true;
            InfantryStrengthText.fontStyle = FontStyles.Bold;
            InfantryStrengthText.text = "UPGRADED";
        }
    }
    public void IncreaseHorsemanStrength()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(6, 3, 0, 0) && !hasUpgradedHorsemanStrength)
        {
            // pridá jednu silu jazdcom
            ResourceManager.Instance.SteelCount -= 6;
            ResourceManager.Instance.BlueCryCount -= 3;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            Unit_Horse.hasIncreasedHorseStrength = true;
            hasUpgradedHorsemanStrength = true;
            foreach (Unit unit in UnitManager.Instance.GetFriendlyUnitList())
            {
                if (unit.TypeOfUnit == Unit.UnitType.Horseman)  unit.IncreaseStrength();
            }
            HorsemanStrengthText.fontStyle = FontStyles.Bold;
            HorsemanStrengthText.text = "UPGRADED";
        }
    }
    public void UpgradeWalls()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(8, 2, 2, 0) && !hasUpgradedWalls)
        {
            // pridá silu hradbám
            Zone.isWallUpgraded = true;
            ResourceManager.Instance.SteelCount -= 8;
            ResourceManager.Instance.BlueCryCount -= 2;
            ResourceManager.Instance.RedCryCount -= 2;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            hasUpgradedWalls = true;
            WallsLvl2Visual.SetActive(true);
            WallLevel1Button.SetActive(false);
            WallLevel2Button.SetActive(true);
        }
    }

    public void UpgradeWallsToLvl2()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(4, 1, 3, 3) && !hasUpgradedWallsLevel2)
        {
            // pridá ešte viac sily hradbám
            Zone.WallLevel2 = true;
            ResourceManager.Instance.SteelCount -= 4;
            ResourceManager.Instance.BlueCryCount -= 1;
            ResourceManager.Instance.RedCryCount -= 3;
            ResourceManager.Instance.CoalCount -= 3;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            hasUpgradedWallsLevel2 = true;
            UpgradedWallsLevel2Text.fontStyle = FontStyles.Bold;
            UpgradedWallsLevel2Text.text = "UPGRADED";
            WallsLvl2Visual.SetActive(false);
            WallsLvl3Visual.SetActive(true);
        }
    }
    public void increaseCoalIncome()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(8, 5, 2, 3) && !hasIncreasedCoalIncome)
        {
            // zvýši príjem uhlia za kolo
            ResourceManager.Instance.CoalIncome += 4;
            ResourceManager.Instance.SteelCount -= 8;
            ResourceManager.Instance.BlueCryCount -= 5;
            ResourceManager.Instance.RedCryCount -= 2;
            ResourceManager.Instance.CoalCount -= 3;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            ResourceVisual.Instance.UpdateResourceIncomeVisual();
            hasIncreasedCoalIncome = true;
            CoalIncomeText.fontStyle = FontStyles.Bold;
            CoalIncomeText.text = "UPGRADED";
        }
    }
    public void IncreaseSteelIncome()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(4, 1, 5, 4) && !hasIncreasedSteelIncome)
        {
            // zvýši príjem železa za kolo
            ResourceManager.Instance.SteelIncome += 2;
            ResourceManager.Instance.SteelCount -= 4;
            ResourceManager.Instance.BlueCryCount -= 1;
            ResourceManager.Instance.RedCryCount -= 5;
            ResourceManager.Instance.CoalCount -= 4;
            ResourceVisual.Instance.UpdateResourceIncomeVisual();
            ResourceVisual.Instance.UpdateResourceCountVisual();
            hasIncreasedSteelIncome = true;
            SteelIncomeText.fontStyle = FontStyles.Bold;
            SteelIncomeText.text = "UPGRADED";
        }
    }
    public void IncreaseNumberOfSoldiersSpawned()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(9, 4, 4, 6) && !hasIncreasedSoldierRecruitment) 
            
        {
            // pridá jednu pechotu za kolo
            AllyUnitSpawner.Instance.anotherUnitSpawned = true;
            ResourceManager.Instance.SteelCount -= 9;
            ResourceManager.Instance.BlueCryCount -= 4;
            ResourceManager.Instance.RedCryCount -= 4;
            ResourceManager.Instance.CoalCount -= 6;
            ResourceVisual.Instance.UpdateResourceCountVisual(); 
            hasIncreasedSoldierRecruitment = true;
            SoldierRecruitmentText.fontStyle = FontStyles.Bold;
            SoldierRecruitmentText.text = "UPGRADED";
        }
    }
    public void IncreaseSoldierTrainingLimit()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(0, 4, 4, 0) && !hasIncreasedSoldierTrainedLimit)
        {
            // zvýši limit na trénovanie jednotiek
            AllyUnitSpawner.Instance.IncreaseMaxUnitSpawnLimit();
            hasIncreasedSoldierTrainedLimit = true;
            ResourceManager.Instance.BlueCryCount -= 4;
            ResourceManager.Instance.RedCryCount -= 4;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            SoldierMaxTrainingText.fontStyle = FontStyles.Bold;
            SoldierMaxTrainingText.text = "UPGRADED";
        }
    }

}
