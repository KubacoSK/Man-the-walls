using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_TankUnit : Unit
{
    // Tank unit that moves further requires fuel and cannot climb walls and also is stronger
    new private void Start()
    {
        base.Start();
        strength = 6;
        ActionPoints = 3;
        maxActionPoints = 3;
        MovementCost = 1;
        canComeToWalls = false;
    }
}

