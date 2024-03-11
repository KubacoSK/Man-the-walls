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
        }
        if (Input.GetKeyDown("h")) 
        {
            AllyUnitSpawner.Instance.SpawnHorse();
        }
    }
}
