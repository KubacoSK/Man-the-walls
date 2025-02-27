
public class Unit_BattleRobot : Unit
{
    // robot s upravenymi statmi
    new void Start()
    {
        base.Start();
        strength = 8;
        ActionPoints = 3;
        maxActionPoints = 3;
        MovementCost = 1;
        TypeOfUnit = UnitType.BattleRobot;
    }
}
