using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zone : MonoBehaviour
{
    private List<Unit> unitsInZone;
    private GridSystemVisual highlighter;
    public static float percentagePopGrowth = 1.05f;
    public static float numberPopGrowth = 0.05f;
    public enum ControlType { allied, enemy, neutral}
    public ControlType whoIsInControl;
    private SpriteRenderer spriteRenderer;
    private Color CurrentColor;
    private bool[] alliedMoveLocationsStatus;                           // we check if there is unit already occupying the zone
    private bool[] enemyMoveLocationsStatus;                            
    private Vector2[] alliedMoveLocations;                              // the positions allied units are able to occupy
    private Vector2[] enemyMoveLocations;                               // the positions enemy units are able to occupy
    float xsize;                                                        // half of x size of a zone
    float ysize;                                                        // half of y size of a zone
    int xpos;                                                           // it is exactly middle position of x
    int ypos;                                                           // it is a little lower from top so the units inside dont cover other zones

    [SerializeField] private Color neutralColor;
    [SerializeField] private Color enemyColor;
    [SerializeField] private Color AllyColor;
    
    [SerializeField] private bool IsWall = false;                  // restriction to movement and buff to defending units
    [SerializeField] private float populationCount = 0.4f;         // Current number of citizens 
    [SerializeField] private bool isPopulated = true;              // if there are any people living inside the zone and population can increase

    [SerializeField] private Slider battleSlider;

    [SerializeField] private int numberOfBlueCrystal = 0;
    public int NumberOfBlueCrystal { get { return numberOfBlueCrystal; } private set { numberOfBlueCrystal = value; } }
    [SerializeField] private int numberOfRedCrystal = 0;
    public int NumberOFRedCrystal { get { return numberOfRedCrystal; } private set { numberOfRedCrystal = value; } }
    [SerializeField] private int numberOfCoal = 0;
    public int NumberOfCoal { get { return numberOfCoal; } private set { numberOfCoal = value; } }
    [SerializeField] public int numberOfSteel = 0;
    public int NumberOfSteel { get { return numberOfSteel; } private set { numberOfSteel = value; } }
    // sets the resources for each tile


    public static event EventHandler ZoneControlChanged;
    public static bool isWallUpgraded;

    private void Awake()
    {
        
        populationCount = Mathf.Round(UnityEngine.Random.Range(populationCount * 0.5f, populationCount * 1.5f) * 100f) / 100f;
        xsize = GetZoneSizeModifier().x / 2;
        ysize = (GetZoneSizeModifier().y / 2);
        
        for (float x = xsize; x >= 0.5; x -= 0.5f)                          // we calculate how many x positions are on a zone
        { 
            xpos++;            
        }
        for (float y = ysize; y >= 0.5; y -= 0.5f)                          // we calculate how many y positions are in a zone
        {
            ypos++;
        }

    }

    private void Start()
    {
        if (battleSlider != null)
        {
            RectTransform sliderRect = battleSlider.GetComponent<RectTransform>();
            battleSlider.transform.position = (Vector3)transform.position - new Vector3(0, (GetZoneSizeModifier().y / 2) - 0.3f, 0);

            // Adjust scale (only X-axis since this is a 2D game)
            battleSlider.transform.localScale = new Vector3(GetZoneSizeModifier().x / 70, battleSlider.transform.localScale.y, 1);
            battleSlider.gameObject.SetActive(false);
        }

        CameraController.CameraSizeChanged += Zone_CameraSizeChanged;
        highlighter = GetComponent<GridSystemVisual>();
        unitsInZone = new List<Unit>();
        whoIsInControl = ControlType.allied;
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentColor = AllyColor;
        
        alliedMoveLocations = new Vector2[xpos * ypos];                           // total number of allied and enemy move positons
        enemyMoveLocations = new Vector2[xpos * ypos];
        alliedMoveLocationsStatus = new bool[xpos * ypos];                          
        enemyMoveLocationsStatus = new bool[xpos * ypos];

        float startX = transform.position.x - 0.25f; // Start X position
        float startY = transform.position.y + ysize - 0.5f; // Start Y position

        // Populate allied move locations
        for (int i = 0; i < alliedMoveLocations.Length; i++)
        {
            float x = startX - (i % xpos) * 0.5f; // Calculate X position based on grid index
            float y = startY - (i / xpos) * 0.8f; // Calculate Y position based on grid index

            alliedMoveLocations[i] = new Vector2(x, y);
            alliedMoveLocationsStatus[i] = false; // Set initial status to false
        }
        float enemyStartX = transform.position.x + 0.25f; 
        for (int i = 0; i < enemyMoveLocations.Length; i++)
        {
            float x = enemyStartX + ((i % xpos) * 0.5f) ; // Calculate X position based on grid index
            float y = startY - (i / xpos) * 0.8f; // Calculate Y position based on grid index

            enemyMoveLocations[i] = new Vector2(x, y);
            enemyMoveLocationsStatus[i] = false; // Set initial status to false
        }


    }
    public void SetAllyPositionStatus(int index,bool status)
    {
        alliedMoveLocationsStatus[index] = status;
    }

    public void SetEnemyPositionStatus(int index, bool status)
    {
        enemyMoveLocationsStatus[index] = status;
    }
    public bool[] GetAllyMoveLocationStatuses() { return alliedMoveLocationsStatus; }
    public Vector2[] GetAllyMoveLocations() { return alliedMoveLocations; }
    public bool[] GetEnemyMoveLocationStatuses() { return enemyMoveLocationsStatus; }
    public Vector2[] GetEnemyMoveLocations() { return enemyMoveLocations; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is a unit
        if (other.CompareTag("Unit") || other.CompareTag("EnemyUnit"))
        {
            // If there is any unit with collider inside the object, add it to the list
            Unit unit = other.GetComponent<Unit>();

            if (unit != null && !unitsInZone.Contains(unit))
            {
                ZoneManager.Instance.AddUnitToZone(unit, this);

            }

            if (ReturnEnemyUnitsInZone().Count > 0 && ReturnAllyUnitsInZone().Count > 0 && battleSlider != null)
            {
                ShowBattleProgressBar();
            }
        }

    }
    public void InitiateEliminationProcess()
    {
        // using this to get units in zone list
        if (GetUnitsInZone().Count >= 2)
        {
            UnitCombat.Instance.TryEliminateUnits(unitsInZone, this);
        }
    }
    public void ShowBattleProgressBar()
    {
        
        Debug.Log("showing battle bar");
        int allyStrength = 0;
        int enemyStrength = 0;
        foreach (Unit unit in ReturnAllyUnitsInZone()) allyStrength += unit.GetStrength(); // increases allied strength based number of allies in zone
        foreach (Unit unit in ReturnEnemyUnitsInZone()) enemyStrength += unit.GetStrength();
        if (IsWall == true) allyStrength += 3; // if we are fighting on a wall we add more power
        if (IsWall && Zone.isWallUpgraded) allyStrength++;
        battleSlider.gameObject.SetActive(true);
        int totalStrength = allyStrength + enemyStrength;
        battleSlider.value = (float)allyStrength / totalStrength;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is a unit
        if (other.CompareTag("Unit") || other.CompareTag("EnemyUnit"))
        {
            Unit unit = other.GetComponent<Unit>();
            ZoneManager.Instance.RemoveUnitFromZone(unit, this);
            // we deactivate the slider if there are no enemy or ally units present
            if (ReturnEnemyUnitsInZone() == null || ReturnAllyUnitsInZone() == null && battleSlider != null) battleSlider.gameObject.SetActive(false);  
        }
    }

    public Vector2 GetZoneSizeModifier()
    {
        Collider2D collider = GetComponent<Collider2D>();

        // Getting the size from the collider's bounds
        Vector2 size = collider.bounds.size;
        return size;

    }

    public void AddUnit(Unit unit)
    {
        unitsInZone.Add(unit);
        // Additional actions you want to perform when a unit enters the zone
    }

    public void RemoveUnit(Unit unit)
    {
        unitsInZone.Remove(unit);
        // Additional actions you want to perform when a unit exits the zone
    }
    public Zone GetClickedZone(Vector3 mouseWorldPosition)
    {
        // checks if you click on an object with collider and if it has zone component
        Collider2D collider = Physics2D.OverlapPoint(mouseWorldPosition, LayerMask.GetMask("GridPoints"));

        if (collider != null)
        {
            return collider.GetComponent<Zone>();
        }

        return null;
    }

    public void Highlight()
    {
        if (highlighter != null)
        {
            highlighter.Highlight(this);
        }
    }

    public void ResetHighlight()
    {
        if (highlighter != null)
        {
            // no idea why this is like it
            highlighter.ResetHighlight(this);
        }
    }
    public List<Unit> GetUnitsInZone()
    {
        return unitsInZone;
    }
    public bool IsWallCheck()
    {
        return IsWall;
    }
    public List<Unit> ReturnAllyUnitsInZone()
    {
        // returns a list of all allied units
        List<Unit> AllyUnits = new List<Unit>();
        foreach (Unit unit in unitsInZone)
        {
            if (!unit.IsEnemy()) AllyUnits.Add(unit);
        }
        return AllyUnits;
    }
    public List<Unit> ReturnEnemyUnitsInZone()
    {
        // returns a list of all allied units
        List<Unit> EnemyUnits = new List<Unit>();
        foreach (Unit unit in unitsInZone)
        {
            if (unit.IsEnemy()) EnemyUnits.Add(unit);
        }
        return EnemyUnits;
    }

    public ControlType WhoIsUnderControl()
    {
        return whoIsInControl;
    }

    public Color ReturnCurrentColor()
    {
        return CurrentColor;
    }

    public void ChangeControlToAlly()
    {
        if (whoIsInControl != ControlType.allied) 
        {
            whoIsInControl = ControlType.allied;
            CurrentColor = AllyColor;
            spriteRenderer.color = CurrentColor;
            ZoneControlChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ChangeControlToEnemy()
    {
        // changes control of a zone if enemy attacks it
        if (whoIsInControl != ControlType.enemy)
        {
            whoIsInControl = ControlType.enemy;
            CurrentColor = enemyColor;
            spriteRenderer.color = CurrentColor;
            populationCount = populationCount * 0.6f;
            ZoneControlChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ChangeControlToNeutral()
    {
        // changes control of a zone if enemy attacks it and there is ally on it or reversed
        if (whoIsInControl != ControlType.neutral)
        {
            whoIsInControl =ControlType.neutral;
            CurrentColor = neutralColor;
            spriteRenderer.color = CurrentColor;
            ZoneControlChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public float GetNumberOfCitizens()
    {
        return populationCount;
    }

    public void PopulationGrowth()
    {
        if (isPopulated)populationCount = populationCount * percentagePopGrowth + numberPopGrowth; // increases population at the start of every turn
        
    }

    public void Zone_CameraSizeChanged(object sender, EventArgs e)
    {
        CurrentColor.a = Mathf.Lerp(0.2f, 0.7f, (Camera.main.orthographicSize - 2f) / 8f); // changes the transparency based on camera zoom
    }

    
}