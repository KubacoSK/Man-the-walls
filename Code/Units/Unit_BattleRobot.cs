using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit_BattleRobot : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        strength = 8;
        ActionPoints = 3;
        maxActionPoints = 3;
    }
}
