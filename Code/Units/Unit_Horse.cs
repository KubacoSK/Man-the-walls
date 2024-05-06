using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_Horse : Unit
{
    // Horse unit that moves further at the cost of ability to climb walls 
    public static bool hasIncreasedHorseStrength;
    new void Start()
    {
        if (hasIncreasedHorseStrength) { strength++; }
        base.Start();
        ActionPoints = 4;
        maxActionPoints = 4;
        canComeToWalls = false;
        TypeOfUnit = UnitType.Horseman;
    }
    

}
    
