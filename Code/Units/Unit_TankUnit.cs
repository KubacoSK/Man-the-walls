using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_TankUnit : Unit
{
    private void Start()
    {
        strength = 6;
        ActionPoints = 3;
        maxActionPoints = 3;
        MovementCost = 1;
        canComeToWalls = false;
    }
}

