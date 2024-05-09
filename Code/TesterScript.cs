using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TesterScript : MonoBehaviour
{
    void Start()
    {
        
    }
    void Update()
    {
        if(Input.GetKeyDown("r"))
        {
            ResourceManager.Instance.SteelCount += 3;
            ResourceManager.Instance.CoalCount += 3;
            ResourceManager.Instance.BlueCryCount += 3;
            ResourceManager.Instance.RedCryCount += 3;
            ResourceVisual.Instance.UpdateResourceCountVisual();
        }
        if (Input.GetKeyDown("h")) 
        {
            AllyUnitSpawner.Instance.SpawnHorse();
        }

        if(Input.GetKeyDown("l")) GameManager.Instance.Lose();
        if (Input.GetKeyDown("p")) GameManager.Instance.Win();
    }
}
