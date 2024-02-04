using Unity.VisualScripting;
using UnityEngine;

public class AllyUnitSpawner : MonoBehaviour
{ 
    public static AllyUnitSpawner Instance;
    [SerializeField] private Unit spawnUnitTemplate;  
    private Vector2 spawnPos = new Vector2(14.1f, 17);
    private int posReset = 0;
    private const float HorizontalSpacing = 0.4f;
    private const int UnitsPerRow = 5;
    private const float RowSpacingX = -2f;
    private const float RowSpacingY = 0.8f;

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

    public void SpawnAllyAtTurn()
    {
        for (float i = ResourceManager.Instance.GetNumberOfTotalPopulation(); i >= 80; i -= 80) // spawn units with offset based of how many of them were spawned
        {
            Debug.Log("Spawning unit based on population" + ResourceManager.Instance.GetNumberOfTotalPopulation());
            spawnPos += new Vector2(HorizontalSpacing, 0f);
            Instantiate(spawnUnitTemplate, spawnPos, Quaternion.identity);
            posReset++;

            if (posReset == UnitsPerRow)
            {
                posReset = 0;
                spawnPos += new Vector2(RowSpacingX, RowSpacingY);
            }
        }
    }
}