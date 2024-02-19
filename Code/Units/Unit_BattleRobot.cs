using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_BattleRobot : Unit
{
    // Battle robot is hybrid between tank and soldier
    void Start()
    {
        strength = 8;
        ActionPoints = 3;
        maxActionPoints = 3;
        MovementCost = 1;
    }
}
