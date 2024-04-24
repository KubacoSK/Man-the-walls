using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;
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
        if (ResourceManager.Instance.DoesItHaveEnoughResources(5, 5, 5, 5))
        {
            Zone.numberPopGrowth = 0.2f;
            Zone.percentagePopGrowth = 1.1f;
        }
    }
    public void IncreaseInfantryStrength()
    {
        
    }
    public void IncreaseHorsemanStrength()
    {

    }
    public void UpgradeWalls()
    {

    }
    public void increaseCoalIncome()
    {

    }

    
}
