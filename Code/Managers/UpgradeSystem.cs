using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static Unit;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;
    private bool hasUpgradedCitizensIncrease;
    private bool hasUpgradedInfantryStrength;
    private bool hasUpgradedHorsemanStrength;
    private bool hasUpgradedWalls;
    private bool hasIncreasedCoalIncome;
    private bool hasIncreasedSteelIncome;
    private bool hasIncreasedSoldierRecruitment;
    private bool hasIncreasedSoldierTrainedLimit;

    [SerializeField] private TextMeshProUGUI CitizensIncreaseText;
    [SerializeField] private TextMeshProUGUI InfantryStrengthText;
    [SerializeField] private TextMeshProUGUI HorsemanStrengthText;
    [SerializeField] private TextMeshProUGUI UpgradedWallsText;
    [SerializeField] private TextMeshProUGUI CoalIncomeText;
    [SerializeField] private TextMeshProUGUI IronIncomeText;
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
        if (ResourceManager.Instance.DoesItHaveEnoughResources(3, 2, 2, 6) && !hasUpgradedCitizensIncrease)
        {
            // increases citizens increases each turn
            ResourceManager.Instance.SteelCount -= 3;
            ResourceManager.Instance.BlueCryCount -= 2;
            ResourceManager.Instance.RedCryCount -= 2;
            ResourceManager.Instance.CoalCount -= 6;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            Zone.numberPopGrowth = 0.2f;
            Zone.percentagePopGrowth = 1.1f;
            hasUpgradedCitizensIncrease = true;
        }
    }
    public void IncreaseInfantryStrength()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(10, 15, 15, 0) && !hasUpgradedInfantryStrength)
        {
            // adds one strength to infantry units
            ResourceManager.Instance.SteelCount -= 10;
            ResourceManager.Instance.BlueCryCount -= 15;
            ResourceManager.Instance.RedCryCount -= 15;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            foreach (Unit unit in UnitManager.Instance.GetFriendlyUnitList()) 
                if (hasIncreasedStrength && unit.TypeOfUnit == UnitType.Infantry) 
                    unit.IncreaseStrength();
            Unit.hasIncreasedStrength = true;
            hasUpgradedInfantryStrength = true;
        }
    }
    public void IncreaseHorsemanStrength()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(6, 3, 0, 0) && !hasUpgradedHorsemanStrength)
        {
            // adds one strength to horse units
            ResourceManager.Instance.SteelCount -= 6;
            ResourceManager.Instance.BlueCryCount -= 3;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            Unit_Horse.hasIncreasedHorseStrength = true;
            hasUpgradedHorsemanStrength = true;
            foreach (Unit unit in UnitManager.Instance.GetFriendlyUnitList())
            {
                if (unit.TypeOfUnit == Unit.UnitType.Horseman)  unit.IncreaseStrength();
            }
        }
    }
    public void UpgradeWalls()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(8, 2, 2, 0) && !hasUpgradedWalls)
        {
            // makes walls stronger
            Zone.isWallUpgraded = true;
            ResourceManager.Instance.SteelCount -= 8;
            ResourceManager.Instance.BlueCryCount -= 2;
            ResourceManager.Instance.RedCryCount -= 2;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            hasUpgradedWalls = true;
        }
    }
    public void increaseCoalIncome()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(8, 5, 2, 3) && !hasIncreasedCoalIncome)
        {
            // increases coal income each round
            ResourceManager.Instance.CoalIncome += 4;
            ResourceManager.Instance.SteelCount -= 8;
            ResourceManager.Instance.BlueCryCount -= 5;
            ResourceManager.Instance.RedCryCount -= 2;
            ResourceManager.Instance.CoalCount -= 3;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            ResourceVisual.Instance.UpdateResourceIncomeVisual();
            hasIncreasedCoalIncome = true;
        }
    }
    public void IncreaseSteelIncome()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(4, 1, 5, 4) && !hasIncreasedSteelIncome)
        {
            // increases steel income each round
            ResourceManager.Instance.SteelIncome += 2;
            ResourceManager.Instance.SteelCount -= 4;
            ResourceManager.Instance.BlueCryCount -= 1;
            ResourceManager.Instance.RedCryCount -= 5;
            ResourceManager.Instance.CoalCount -= 4;
            ResourceVisual.Instance.UpdateResourceIncomeVisual();
            ResourceVisual.Instance.UpdateResourceCountVisual();
            hasIncreasedSteelIncome = true;
        }
    }
    public void IncreaseNumberOfSoldiersSpawned()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(8, 3, 3, 5) && !hasIncreasedSoldierRecruitment) 
            
        {
            AllyUnitSpawner.Instance.anotherUnitSpawned = true;
            ResourceManager.Instance.SteelCount -= 8;
            ResourceManager.Instance.BlueCryCount -= 3;
            ResourceManager.Instance.RedCryCount -= 3;
            ResourceManager.Instance.CoalCount -= 5;
            ResourceVisual.Instance.UpdateResourceCountVisual(); 
            hasIncreasedSoldierRecruitment = true;
            // adds one soldier to spawn at start of a turn
        }
    }
    public void IncreaseSoldierTrainingLimit()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(0, 4, 4, 0) && !hasIncreasedSoldierTrainedLimit)
        {
            AllyUnitSpawner.Instance.IncreaseMaxUnitSpawnLimit();
            hasIncreasedSoldierTrainedLimit = true;
            ResourceManager.Instance.BlueCryCount -= 4;
            ResourceManager.Instance.RedCryCount -= 4;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            // unlocks creating one more paid soldier at every turn 
        }
    }

}
