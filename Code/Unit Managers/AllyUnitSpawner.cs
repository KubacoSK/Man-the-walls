using UnityEngine;

public class AllyUnitSpawner : MonoBehaviour
{ 
    public static AllyUnitSpawner Instance;
    [SerializeField] private Unit tankUnitPrefab;
    [SerializeField] private Unit battleRobotPrefab;
    [SerializeField] private Unit horseUnitPrefab;
    [SerializeField] private Unit spawnUnitPrefab;
    [SerializeField] private Unit superPetrossPrefab;
    [SerializeField] private Zone spawnCenterZone;
    private Vector2 spawnPosition;

    private int SpawnedPaidUnitsThisTurn = 0;
    private int PaidUnitsSpawnLimit = 1;
    private int HorseSpawnsTillPetrossSpawn = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one AllyUnitSpawner! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void ResetPaidSpawnedUnits()
    {
        SpawnedPaidUnitsThisTurn = 0;
    }
    public void SpawnAllyAtTurn()
    {
        for (float i = ResourceManager.Instance.GetNumberOfTotalPopulation(); i >= 80; i -= 80) // spawn units with offset based of how many of them were spawned
        {
            int index = 0;
            for (int k = 0; k < spawnCenterZone.GetAllyMoveLocationStatuses().Length; k++) // we find the first available position in the list
            {
                if (spawnCenterZone.GetAllyMoveLocationStatuses()[k] == false)
                {
                    spawnPosition = spawnCenterZone.GetAllyMoveLocations()[k];
                    spawnCenterZone.SetAllyPositionStatus(k, true);
                    Unit unit = Instantiate(spawnUnitPrefab, spawnPosition, Quaternion.identity);
                    unit.SetStandingZone(spawnCenterZone, k);
                    index = k;
                    break;
                    
                }

            }
        }
    }

    public void SpawnTank()
    {
        if (DoesItHaveEnoughResources(4, 0 ,0) && SpawnedPaidUnitsThisTurn < PaidUnitsSpawnLimit)
        {
            int index = 0;
            for (int k = 0; k < spawnCenterZone.GetAllyMoveLocationStatuses().Length; k++) // we find the first available position in the list
            {
                if (spawnCenterZone.GetAllyMoveLocationStatuses()[k] == false)
                {
                    spawnPosition = spawnCenterZone.GetAllyMoveLocations()[k];
                    spawnCenterZone.SetAllyPositionStatus(k, true);
                    Unit unit = Instantiate(tankUnitPrefab, spawnPosition, Quaternion.identity);
                    unit.SetStandingZone(spawnCenterZone, k);
                    index = k;
                    break;

                }

            }
            SpawnedPaidUnitsThisTurn += 1;
            ResourceManager.Instance.SteelCount -= 4;
            ResourceVisual.Instance.UpdateResourceCountVisual();
        }
    }

    public void SpawnBattleRobot()
    {
        if (DoesItHaveEnoughResources(3, 1, 1) && SpawnedPaidUnitsThisTurn < PaidUnitsSpawnLimit)
        {
            int index = 0;
            for (int k = 0; k < spawnCenterZone.GetAllyMoveLocationStatuses().Length; k++) // we find the first available position in the list
            {
                if (spawnCenterZone.GetAllyMoveLocationStatuses()[k] == false)
                {
                    spawnPosition = spawnCenterZone.GetAllyMoveLocations()[k];
                    spawnCenterZone.SetAllyPositionStatus(k, true);
                    Unit unit = Instantiate(battleRobotPrefab, spawnPosition, Quaternion.identity);
                    unit.SetStandingZone(spawnCenterZone, k);
                    index = k;
                    break;

                }

            }
            SpawnedPaidUnitsThisTurn += 1;
            ResourceManager.Instance.SteelCount -= 3;
            ResourceManager.Instance.BlueCryCount -= 1;
            ResourceManager.Instance.RedCryCount -= 1;
            ResourceVisual.Instance.UpdateResourceCountVisual();
        }
    }

    public void SpawnHorse()
    {
        if (DoesItHaveEnoughResources(1, 0, 0) && SpawnedPaidUnitsThisTurn < PaidUnitsSpawnLimit)
        {
            int index = 0;
            for (int k = 0; k < spawnCenterZone.GetAllyMoveLocationStatuses().Length; k++) // we find the first available position in the list
            {
                if (spawnCenterZone.GetAllyMoveLocationStatuses()[k] == false)
                {
                    spawnPosition = spawnCenterZone.GetAllyMoveLocations()[k];                  // we spawn unit on found position
                    spawnCenterZone.SetAllyPositionStatus(k, true);
                    Unit unit = Instantiate(horseUnitPrefab, spawnPosition, Quaternion.identity);                    
                    unit.SetStandingZone(spawnCenterZone, k);                        // we set the spawned units zone and position so when they move they return it to default position
                    index = k;
                    break;

                }

            }
            SpawnedPaidUnitsThisTurn += 1;
            ResourceManager.Instance.SteelCount -= 1;
            ResourceVisual.Instance.UpdateResourceCountVisual();
            HorseSpawnsTillPetrossSpawn++;
            if (HorseSpawnsTillPetrossSpawn == 5)
            {
                Instantiate(superPetrossPrefab, new Vector2(12.5f, 19.5f), Quaternion.identity);
            }
        }
    }
    private bool DoesItHaveEnoughResources(int Steel, int Bcrys, int Rcrys)
    {
        if (ResourceManager.Instance.SteelCount >= Steel &&         // we check if we have higher or same amount of resources required
            ResourceManager.Instance.BlueCryCount >= Bcrys &&
            ResourceManager.Instance.RedCryCount >= Rcrys) 
        {
            return true;
        }
        return false;
    }
}