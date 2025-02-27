using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Zone : MonoBehaviour
{
    private List<Unit> unitsInZone;                                     // list jednotiek v zóne
    private GridSystemVisual highlighter;
    public static float percentagePopGrowth = 1.05f;                    // percentuálne kolko jednotiek pribúda za kolo
    public static float numberPopGrowth = 0.05f;                        // staticky kolko ludí pribúda v zóne za kolo
    public enum ControlType { allied, enemy, neutral}                  
    public ControlType whoIsInControl;
    private SpriteRenderer spriteRenderer;
    private Color CurrentColor;
    private bool[] alliedMoveLocationsStatus;                           // stavy miest na pohyb
    private bool[] enemyMoveLocationsStatus;                            
    private Vector2[] alliedMoveLocations;                              // miesta kam sa naše jednotky vedia pohnúť
    private Vector2[] enemyMoveLocations;                               // miesta kam sa nepriateľské jednotky vedia pohnúť
    float xsize;                                                        // polovica x veľkosti zóny
    float ysize;                                                        // polovica y veľkosti zóny
    int xpos;                                                           // x stred zóny
    int ypos;                                                           // od kade vedia zóny sa pohnúť v osi y

    [SerializeField] private Color neutralColor;
    [SerializeField] private Color enemyColor;
    [SerializeField] private Color AllyColor;

    
    [SerializeField] private bool IsWall = false;                  // restriction to movement and buff to defending units
    [SerializeField] private float populationCount = 0.4f;         // Current number of citizens 
    [SerializeField] private bool isPopulated = true;              // if there are any people living inside the zone and population can increase

    [SerializeField] private Slider battleSlider;


    // kolko zóna zarába kryštáľov
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
    public static bool WallLevel2;
    private bool isCombatActive;                            // či boj práve prebieha v zóne

    private void Awake()
    {
        
        populationCount = Mathf.Round(UnityEngine.Random.Range(populationCount * 0.5f, populationCount * 1.5f) * 100f) / 100f;
        xsize = GetZoneSizeModifier().x / 2;
        ysize = (GetZoneSizeModifier().y / 2);
        
        for (float x = xsize; x >= 0.5; x -= 0.5f)                          // vypočítame koľko x pozícií je v zóne
        { 
            xpos++;            
        }
        for (float y = ysize; y >= 0.5; y -= 0.5f)                          // vypočítame koľko y pozícií je v zóne
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

            // prisposobime veľkosť slajdera podľa veľkosti zóny
            battleSlider.transform.localScale = new Vector3(GetZoneSizeModifier().x / 70, battleSlider.transform.localScale.y, 1);
            battleSlider.gameObject.SetActive(false);
        }

        // nastavíme hodnoty v prvom frame
        CameraController.CameraSizeChanged += Zone_CameraSizeChanged;
        highlighter = GetComponent<GridSystemVisual>();
        unitsInZone = new List<Unit>();
        whoIsInControl = ControlType.allied;
        spriteRenderer = GetComponent<SpriteRenderer>();
        CurrentColor = AllyColor;
        
        alliedMoveLocations = new Vector2[xpos * ypos];                           // celkový počet spojeneckých a nepriateľských lokácií na pohyb
        enemyMoveLocations = new Vector2[xpos * ypos];
        alliedMoveLocationsStatus = new bool[xpos * ypos];                          
        enemyMoveLocationsStatus = new bool[xpos * ypos];

        float startX = transform.position.x - 0.25f; // štartovná X position
        float startY = transform.position.y + ysize - 0.5f; // Štartovná Y position

        // Populate allied move locations
        for (int i = 0; i < alliedMoveLocations.Length; i++)
        {
            float x = startX - (i % xpos) * 0.5f; // Spočítame x pozíciu podla indexu
            float y = startY - (i / xpos) * 0.8f; // Spočítame y pozíciu podla indexu

            alliedMoveLocations[i] = new Vector2(x, y);
            alliedMoveLocationsStatus[i] = false; // nastavíme všetky obsadenia na neobsadené
        }
        float enemyStartX = transform.position.x + 0.25f; 
        for (int i = 0; i < enemyMoveLocations.Length; i++)
        {
            float x = enemyStartX + ((i % xpos) * 0.5f) ;
            float y = startY - (i / xpos) * 0.8f; // to isté len pre nepriateľov

            enemyMoveLocations[i] = new Vector2(x, y);
            enemyMoveLocationsStatus[i] = false; 
        }


    }
    void Update()
    {
        if (isCombatActive)
        {
            Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
            bool isVisible = screenPoint.x >= 0 && screenPoint.x <= 1 && screenPoint.y >= 0 && screenPoint.y <= 1;

            if (isVisible)
                CombatSound.Instance.PlayCombatSounds();
            else
                CombatSound.Instance.StopCombatSounds();
        }
    }
    public void SetAllyPositionStatus(int index,bool status)
    {
        alliedMoveLocationsStatus[index] = status;  // nastavíme že jedno z volných miest je zabrané
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
        // zistíme či vstupujúci objekt je jednotka
        if (other.CompareTag("Unit") || other.CompareTag("EnemyUnit"))
        {
            // Ak je, pridáme ju do listu
            Unit unit = other.GetComponent<Unit>();

            if (unit != null && !unitsInZone.Contains(unit))
            {
                ZoneManager.Instance.AddUnitToZone(unit, this);

            }
            // zistíme že či sa v zóne nachádzajú aj nepriateľské jednotky a začneme boj
            if (ReturnEnemyUnitsInZone().Count > 0 && ReturnAllyUnitsInZone().Count > 0 && battleSlider != null)  
            {

                foreach (Unit sigma in ReturnAllyUnitsInZone())
                {
                    sigma.SetShootingAnimation(true);
                    isCombatActive = true;
                }
                foreach (Unit sigma in ReturnEnemyUnitsInZone())
                {
                    sigma.SetShootingAnimation(true);
                    isCombatActive = true;
                }
                ShowBattleProgressBar();
            }
        }

    }
    public void ChangeCombatStatus(bool active)
    {
        isCombatActive = active;
    }
    public void InitiateEliminationProcess()
    {
        // toto if zavolá metódu na vymazanie jednotiek ktoré padli v boji
        if (GetUnitsInZone().Count >= 2)
        {
            UnitCombat.Instance.TryEliminateUnits(unitsInZone, this);
        }
    }
    public void ShowBattleProgressBar()
    {
        // vypočíta sily vojsk prítomných na zóne
        int allyStrength = 0;
        int enemyStrength = 0;
        foreach (Unit unit in ReturnAllyUnitsInZone()) allyStrength += unit.GetStrength(); // podľa počtu a typu jednotiek pridá silu
        foreach (Unit unit in ReturnEnemyUnitsInZone()) enemyStrength += unit.GetStrength();
        if (IsWall == true) allyStrength += 2; // na hradbách sa pridá ešte viac sily nepriateľom
        if (IsWall && Zone.isWallUpgraded) allyStrength++;
        if (IsWall && Zone.WallLevel2) allyStrength++;
        battleSlider.gameObject.SetActive(true);  // aktivuje battle slider
        int totalStrength = allyStrength + enemyStrength;
        battleSlider.value = (float)allyStrength / totalStrength; // upravíme slajder tak aby ukazoval proporcionálnu silu vojsk
    }
    public void HideBattleProgressBar()
    {
        battleSlider.gameObject.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Unit") || other.CompareTag("EnemyUnit"))
        {
            Unit unit = other.GetComponent<Unit>();
            ZoneManager.Instance.RemoveUnitFromZone(unit, this);
            // zistíme že či stále sú v zóne nažive nejaké jednotky, ak nie, tak sa slajder vypne
            if (ReturnEnemyUnitsInZone() == null || ReturnAllyUnitsInZone() == null && battleSlider != null) battleSlider.gameObject.SetActive(false);  
        }
    }

    public Vector2 GetZoneSizeModifier()
    {
        Collider2D collider = GetComponent<Collider2D>();

        // zistíme veľkost zóny
        Vector2 size = collider.bounds.size;
        return size;

    }

    public void AddUnit(Unit unit)
    {
        unitsInZone.Add(unit);  // pridá jednotku do listu prítomných jednotiek

    }

    public void RemoveUnit(Unit unit)
    {
        unitsInZone.Remove(unit);
    }
    public Zone GetClickedZone(Vector3 mouseWorldPosition)
    {
        // Zistíme či klikáme na zónu s colliderom a že či to je vlastne zóna
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
            // toto je zahada
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
        List<Unit> AllyUnits = new List<Unit>();
        foreach (Unit unit in unitsInZone)
        {
            if (!unit.IsEnemy()) AllyUnits.Add(unit);
        }
        return AllyUnits;
    }
    public List<Unit> ReturnEnemyUnitsInZone()
    {
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
        if (whoIsInControl != ControlType.allied)   // zmení kontrolu nad zónou na spojeneckú, popri tom aj uprví farbu
        {
            whoIsInControl = ControlType.allied;
            CurrentColor = AllyColor;
            spriteRenderer.color = CurrentColor;
            ZoneControlChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public void ChangeControlToEnemy()
    {
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
        // ak sú v zóne jednotky oboch strán tak sa zmení na neutrálnu
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
        if (isPopulated)populationCount = populationCount * percentagePopGrowth + numberPopGrowth; // Zvýši populáciu na začiatku každého kola
        
    }

    public void Zone_CameraSizeChanged(object sender, EventArgs e)
    {
        CurrentColor.a = Mathf.Lerp(0.2f, 0.7f, (Camera.main.orthographicSize - 2f) / 8f); // zmení priesvitnosť podľa toho, ako veľmi zoomnuté to máme
    }

    
}