using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    public static UpgradeSystem Instance;
    bool CitizensIncrease;
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

    }
    public void IncreaseInfantryStrength()
    {
        
    }
    public void IncreaseHorsemanStrength()
    {

    }
}
