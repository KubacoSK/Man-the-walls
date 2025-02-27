
public class Unit_Horse : Unit
{
    // upraveny kon
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
    
