

public class Unit_TankUnit : Unit
{
    // zmeny pre tank jednotku
    new private void Start()
    {
        base.Start();
        strength = 6;
        ActionPoints = 3;
        maxActionPoints = 3;
        MovementCost = 1;
        canComeToWalls = false;
        TypeOfUnit = UnitType.Tank;
    }
}

