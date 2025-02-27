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

    private int SpawnedPaidUnitsThisTurn = 0; // pocet jednotiek, ktore boli tento tah zaplatene a vytvorene
    private int PaidUnitsSpawnLimit = 1; // maximalny pocet platenych jednotiek, ktore mozno vytvorit za tah
    private int HorseSpawnsTillPetrossSpawn = 0; // pocet vytvorenych koni do vytvorenia specialnej jednotky Petross
    public bool anotherUnitSpawned = false; // ci bola vytvorena dalsia jednotka

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existuje viac ako jedna instancia AllyUnitSpawner! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void ResetPaidSpawnedUnits()
    {
        SpawnedPaidUnitsThisTurn = 0; // resetuje pocet platenych jednotiek na zaciatku tahu
    }

    public void SpawnAllyAtTurn()
    {
        for (float i = ResourceManager.Instance.GetNumberOfTotalPopulation(); i >= 80; i -= 80)
        {
            int index = 0;
            for (int k = 0; k < spawnCenterZone.GetAllyMoveLocationStatuses().Length; k++) // najdeme prvu volnu poziciu v zozname
            {
                if (spawnCenterZone.GetAllyMoveLocationStatuses()[k] == false)
                {
                    // spawn jednotky a nastavenie jej pozicie tak, aby sa po starte resetovala
                    spawnPosition = spawnCenterZone.GetAllyMoveLocations()[k];
                    spawnCenterZone.SetAllyPositionStatus(k, true);
                    Unit unit = Instantiate(spawnUnitPrefab, spawnPosition, Quaternion.identity);
                    unit.SetStandingZone(spawnCenterZone, k);
                    index = k;
                    break;
                }
            }
        }
        if (anotherUnitSpawned)
        {
            for (int k = 0; k < spawnCenterZone.GetAllyMoveLocationStatuses().Length; k++) // najdeme prvu volnu poziciu v zozname
            {
                if (spawnCenterZone.GetAllyMoveLocationStatuses()[k] == false)
                {
                    // spawn jednotky a nastavenie jej pozicie
                    spawnPosition = spawnCenterZone.GetAllyMoveLocations()[k];
                    spawnCenterZone.SetAllyPositionStatus(k, true);
                    Unit unit = Instantiate(spawnUnitPrefab, spawnPosition, Quaternion.identity);
                    unit.SetStandingZone(spawnCenterZone, k);
                    break;
                }
            }
        }
    }

    public void SpawnTank()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(4, 0, 0, 0) && SpawnedPaidUnitsThisTurn < PaidUnitsSpawnLimit)
        {
            for (int k = 0; k < spawnCenterZone.GetAllyMoveLocationStatuses().Length; k++) // najdeme prvu volnu poziciu v zozname
            {
                if (spawnCenterZone.GetAllyMoveLocationStatuses()[k] == false)
                {
                    spawnPosition = spawnCenterZone.GetAllyMoveLocations()[k];
                    spawnCenterZone.SetAllyPositionStatus(k, true);
                    Unit unit = Instantiate(tankUnitPrefab, spawnPosition, Quaternion.identity);
                    unit.SetStandingZone(spawnCenterZone, k);
                    break;
                }
            }
            SpawnedPaidUnitsThisTurn += 1;
            ResourceManager.Instance.SteelCount -= 4; // odpocitanie zdrojov
            ResourceVisual.Instance.UpdateResourceCountVisual(); // aktualizacia vizualu zdrojov
        }
    }

    public void SpawnBattleRobot()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(3, 1, 1, 0) && SpawnedPaidUnitsThisTurn < PaidUnitsSpawnLimit)
        {
            for (int k = 0; k < spawnCenterZone.GetAllyMoveLocationStatuses().Length; k++) // najdeme prvu volnu poziciu v zozname
            {
                if (spawnCenterZone.GetAllyMoveLocationStatuses()[k] == false)
                {
                    spawnPosition = spawnCenterZone.GetAllyMoveLocations()[k];
                    spawnCenterZone.SetAllyPositionStatus(k, true);
                    Unit unit = Instantiate(battleRobotPrefab, spawnPosition, Quaternion.identity);
                    unit.SetStandingZone(spawnCenterZone, k);
                    break;
                }
            }
            SpawnedPaidUnitsThisTurn += 1;
            ResourceManager.Instance.SteelCount -= 3;
            ResourceManager.Instance.BlueCryCount -= 1;
            ResourceManager.Instance.RedCryCount -= 1;
            ResourceVisual.Instance.UpdateResourceCountVisual(); // aktualizacia vizualu zdrojov
        }
    }

    public void SpawnHorse()
    {
        if (ResourceManager.Instance.DoesItHaveEnoughResources(1, 0, 0, 0) && SpawnedPaidUnitsThisTurn < PaidUnitsSpawnLimit)
        {
            for (int k = 0; k < spawnCenterZone.GetAllyMoveLocationStatuses().Length; k++) // najdeme prvu volnu poziciu v zozname
            {
                if (spawnCenterZone.GetAllyMoveLocationStatuses()[k] == false)
                {
                    spawnPosition = spawnCenterZone.GetAllyMoveLocations()[k]; // spawn jednotky na najdenej pozicii
                    spawnCenterZone.SetAllyPositionStatus(k, true);
                    Unit unit = Instantiate(horseUnitPrefab, spawnPosition, Quaternion.identity);
                    unit.SetStandingZone(spawnCenterZone, k); // nastavenie zony a pozicie jednotky, aby sa po pohybe vratila na defaultnu poziciu
                    break;
                }
            }
            SpawnedPaidUnitsThisTurn += 1;
            ResourceManager.Instance.SteelCount -= 1;
            ResourceVisual.Instance.UpdateResourceCountVisual(); // aktualizacia vizualu zdrojov
            HorseSpawnsTillPetrossSpawn++;
            if (HorseSpawnsTillPetrossSpawn == 8)
            {
                Instantiate(superPetrossPrefab, new Vector2(12.5f, 19.5f), Quaternion.identity); // spawn specialnej jednotky Petross po 8 konoch
            }
        }
    }

    public void IncreaseMaxUnitSpawnLimit()
    {
        PaidUnitsSpawnLimit++; // zvysenie limitu platenych jednotiek na tah
    }
}
