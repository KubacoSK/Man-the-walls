using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionsSystem : MonoBehaviour
{
    public static UnitActionsSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    private Unit selectedUnit;
    private bool IsMoving = false;                          // kontrola, či sa jednotka momentálne pohybuje
    Vector2 destination;                                    // vektor cieľovej pozície, kam jednotka smeruje (pozícia v poli pozícií)
    private float lastTapTime = 0f;
    private const float doubleTapThreshold = 0.3f;
    [SerializeField] private AudioSource selectSound;
    [SerializeField] private TextMeshProUGUI numberOfActionPoints;  // premenná na zobrazenie počtu akčných bodov vybratej jednotky

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Existuje viac ako jeden UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) numberOfActionPoints.text = "";  // skrytie akčných bodov, ak nie je ťah hráča
        if (TurnSystem.Instance.IsPlayerTurn() && !PauseMenu.IsGamePaused())
        {
            if (selectedUnit != null)
            {
                // Kontrola, či jednotka dosiahla cieľovú pozíciu
                if (Vector2.Distance(selectedUnit.transform.position, destination) < 0.01f)
                {
                    if (IsMoving) // Aktualizácia iba ak sa stav zmenil
                    {
                        IsMoving = false;
                        selectedUnit.SetRunningAnimation(false);
                        selectedUnit.FlipUnitBack();
                    }
                }
            }

            // Kontrola kliknutia na jednotku alebo pohyb
            if (Input.GetMouseButtonDown(0) && IsMoving == false)
            {
                float timeSinceLastTap = Time.time - lastTapTime;
                if (TryHandleUnitSelection()) return;
                else if (timeSinceLastTap <= doubleTapThreshold)
                {
                    Movement();
                }
                else lastTapTime = Time.time;
            }
        }
    }

    private void Movement()
    {
        if (selectedUnit != null)
        {
            float timeSinceLastTap = Time.time - lastTapTime;
            if (Input.GetMouseButtonDown(0) && IsMoving == false && selectedUnit.GetActionPoints() > 0 && selectedUnit != null && CanSteamMachineMove())
            {
                Vector3 mouseWorldPosition = MouseWorld.GetPosition();
                Zone clickedZone = GetClickedZone(mouseWorldPosition);

                if (clickedZone != null)
                {
                    selectedUnit.SetPastZoneBack();
                    destination = new Vector2();

                    if (IsValidClickedZone(clickedZone, selectedUnit.GetMoveAction().GetValidZonesList()))
                    {
                        int index = 0;
                        for (int i = 0; i < clickedZone.GetAllyMoveLocationStatuses().Length; i++)
                        {
                            if (clickedZone.GetAllyMoveLocationStatuses()[i] == false)
                            {
                                destination = clickedZone.GetAllyMoveLocations()[i];
                                clickedZone.SetAllyPositionStatus(i, true);
                                index = i;
                                break;
                            }
                        }

                        if (selectedUnit != null)
                        {
                            if (destination.x < selectedUnit.transform.position.x) selectedUnit.FlipUnit();
                            IsMoving = true;
                            selectedUnit.SetRunningAnimation(true); // Spustenie animácie behu
                            selectedUnit.GetMoveAction().Move(destination);
                            selectedUnit.DoAction(clickedZone);
                            selectedUnit.SetStandingZone(clickedZone, index);
                            ResourceManager.Instance.CoalCount -= selectedUnit.GetMovementCost();
                            ResourceVisual.Instance.UpdateResourceCountVisual();
                            numberOfActionPoints.text = "Action points: " + selectedUnit.GetActionPoints();

                            if (clickedZone.ReturnEnemyUnitsInZone().Count > 0)
                            {
                                clickedZone.ChangeControlToNeutral();
                            }
                            else
                            {
                                clickedZone.ChangeControlToAlly();
                            }
                        }
                    }
                }
            }
        }
    }

    private bool IsValidClickedZone(Zone clickedZone, List<Zone> validZones)
    {
        // kontrola, či je kliknutá zóna medzi platnými zónami
        return validZones.Contains(clickedZone);
    }

    private bool TryHandleUnitSelection()
    {
        // Vystrelenie lúča z kamery na zistenie, či sme klikli na jednotku
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = LayerMask.GetMask("Units");
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            if (hit.transform.TryGetComponent<Unit>(out Unit unit) && !unit.IsEnemy())
            {
                selectSound.Play();
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        // vyvola udalosť ak vyberieme inú jednotku a nastaví ju ako vybranú
        selectedUnit = unit;
        if (selectedUnit.IsEnemy())
        {

        }
        numberOfActionPoints.text = "Action points: " + selectedUnit.GetActionPoints();
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool CanSteamMachineMove()
    {
        // kontrola, či máme dostatok uhlia na pohyb jednotky
        if (ResourceManager.Instance.CoalCount >= selectedUnit.GetMovementCost()) return true;
        else return false;
    }
    private Zone GetClickedZone(Vector3 mouseWorldPosition)
    {
        // zistenie, na ktorú zónu bolo kliknuté
        Collider2D collider = Physics2D.OverlapPoint(mouseWorldPosition, LayerMask.GetMask("GridPoints"));

        if (collider != null)
        {
            return collider.GetComponent<Zone>();
        }

        return null;
    }
    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}
