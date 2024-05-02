using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

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
        if (ResourceManager.Instance.DoesItHaveEnoughResources(5, 5, 5, 5) && !hasUpgradedCitizensIncrease)
        {
            Zone.numberPopGrowth = 0.2f;
            Zone.percentagePopGrowth = 1.1f;
            hasUpgradedCitizensIncrease = true;
        }
    }
    public void IncreaseInfantryStrength()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(5, 5, 5, 5) && !hasUpgradedInfantryStrength)
        {
            Zone.numberPopGrowth = 0.2f;
            Zone.percentagePopGrowth = 1.1f;
            hasUpgradedInfantryStrength = true;
        }
    }
    public void IncreaseHorsemanStrength()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(4, 2, 0, 0) && !hasUpgradedHorsemanStrength)
        {
            
            hasUpgradedHorsemanStrength = true;
        }
    }
    public void UpgradeWalls()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(5, 5, 5, 5) && !hasUpgradedWalls)
        {
            
            hasUpgradedWalls = true;
        }
    }
    public void increaseCoalIncome()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(5, 5, 5, 5) && !hasIncreasedCoalIncome)
        {
            ResourceManager.Instance.CoalIncome += 4;
            ResourceVisual.Instance.UpdateResourceIncomeVisual();
            hasIncreasedCoalIncome = true;
        }
    }
    public void IncreaseSteelIncome()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(4, 1, 5, 4) && !hasIncreasedSteelIncome)
        {
            ResourceManager.Instance.SteelIncome += 2;
            ResourceVisual.Instance.UpdateResourceIncomeVisual();
        }
    }
    public void IncreaseNumberOfSoldiersSpawned()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(8, 3, 3, 5) && !hasIncreasedSoldierRecruitment)
        {
            
            hasIncreasedSoldierRecruitment = true;
        }
    }
    public void IncreaseSoldierTrainingLimit()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(0, 4, 4, 0) && !hasIncreasedSoldierTrainedLimit)
        {
            
            hasIncreasedSoldierTrainedLimit = true;
        }
    }

}
