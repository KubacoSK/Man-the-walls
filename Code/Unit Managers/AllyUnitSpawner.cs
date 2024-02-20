using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AllyUnitSpawner : MonoBehaviour
{ 
    public static AllyUnitSpawner Instance;
    [SerializeField] private Unit tankUnitPrefab;
    [SerializeField] private Unit horseUnitPrefab;
    [SerializeField] private Unit spawnUnitPrefab;
    [SerializeField] private Unit superPetrossPrefab;
    private Vector2 spawnPos = new Vector2(14.1f, 17);
    private int posReset = 0;
    private const float HorizontalSpacing = 0.4f;
    private const int UnitsPerRow = 5;
    private const float RowSpacingX = -2f;
    private const float RowSpacingY = 0.8f;

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
            Debug.Log("Spawning unit based on population" + ResourceManager.Instance.GetNumberOfTotalPopulation());
            spawnPos += new Vector2(HorizontalSpacing, 0f);
            Instantiate(spawnUnitPrefab, spawnPos, Quaternion.identity);
            posReset++;

            if (posReset == UnitsPerRow)
            {
                posReset = 0;
                spawnPos += new Vector2(RowSpacingX, RowSpacingY);
            }
        }
    }

    public void SpawnTank()
    {
        if (DoesItHaveEnoughResources(4, 0 ,0) && SpawnedPaidUnitsThisTurn < PaidUnitsSpawnLimit)
        {
            Instantiate(tankUnitPrefab, new Vector2(16,16), Quaternion.identity);
            SpawnedPaidUnitsThisTurn += 1;
            ResourceManager.Instance.SteelCount -= 4;
        }
    }

    public void SpawnBattleRobot()
    {
        if (DoesItHaveEnoughResources(3, 1, 1) && SpawnedPaidUnitsThisTurn < PaidUnitsSpawnLimit)
        {
            Instantiate(tankUnitPrefab, new Vector2(16, 16), Quaternion.identity);
            SpawnedPaidUnitsThisTurn += 1;
            ResourceManager.Instance.SteelCount -= 3;
            ResourceManager.Instance.BlueCryCount -= 1;
            ResourceManager.Instance.RedCryCount -= 1;
        }
    }

    public void SpawnHorse()
    {
        if (DoesItHaveEnoughResources(1, 0, 0) && SpawnedPaidUnitsThisTurn < PaidUnitsSpawnLimit)
        {
            Instantiate(horseUnitPrefab, new Vector2(16, 16), Quaternion.identity);
            SpawnedPaidUnitsThisTurn += 1;
            ResourceManager.Instance.SteelCount -= 1;
            HorseSpawnsTillPetrossSpawn++;
            if (HorseSpawnsTillPetrossSpawn == 5)
            {
                Instantiate(superPetrossPrefab, new Vector2(12.5f, 19.5f), Quaternion.identity);
            }
        }
    }
    private bool DoesItHaveEnoughResources(int Steel, int Bcrys, int Rcrys)
    {
        if (ResourceManager.Instance.SteelCount >= Steel &&
            ResourceManager.Instance.BlueCryCount >= Bcrys &&
            ResourceManager.Instance.RedCryCount >= Rcrys) 
        {
            return true;
        }
        return false;
    }
}