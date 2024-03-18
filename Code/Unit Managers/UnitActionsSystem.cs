using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionsSystem : MonoBehaviour
{
    public static UnitActionsSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;
    private Unit selectedUnit;
    private bool IsMoving = false;                          // check if unit is currently moving 
    Vector2 destination;                                    // vector of destination the unit is heading towards ( its a position for the positions array)

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one UnitActionSystem! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn() && !PauseMenu.IsGamePaused())
        {
            if (selectedUnit != null)
            {

                // checks if unit based on distance to target zone
                if (Vector2.Distance(selectedUnit.transform.position, destination) < 0.01f)
                {
                    IsMoving = false;
                }
            }
            //checks for clicking on unit
            if (Input.GetMouseButtonDown(0) && IsMoving == false)
            {
                if (TryHandleUnitSelection()) return;
            }

            Movement();
        }
    }

    private void Movement()
    {
        if (selectedUnit != null)
        {
            if (Input.GetMouseButtonDown(1) && IsMoving == false && selectedUnit.GetActionPoints() > 0 && selectedUnit != null && CanSteamMachineMove())
            {
                
                // Right-click to move the selected unit
                Vector3 mouseWorldPosition = MouseWorld.GetPosition();
                Zone clickedZone = GetClickedZone(mouseWorldPosition);

                // checks if we clicked on zone and not on some empty space
                if (clickedZone != null)
                {
                    selectedUnit.SetPastZoneBack();                                      // we set the previous position to false
                    destination = new Vector2();
                    // gets list of close zones from MoveAction class
                    if (IsValidClickedZone(clickedZone, selectedUnit.GetMoveAction().GetValidZonesList()))
                    {
                        int index = 0;
                        for (int i = 0; i < clickedZone.GetAllyMoveLocationStatuses().Length; i++) // we find the first available position in the list
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
                            // moves to to position
                            IsMoving = true;
                            selectedUnit.GetMoveAction().Move(destination);  // we move to the found position
                            Debug.Log(destination);
                            selectedUnit.DoAction(clickedZone);
                            selectedUnit.SetStandingZone(clickedZone, index);
                            ResourceManager.Instance.CoalCount -= selectedUnit.GetMovementCost();
                            ResourceVisual.Instance.UpdateResourceCountVisual();

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
        // chcecks if zone is among valid zones list
        return validZones.Contains(clickedZone);
    }

    private bool TryHandleUnitSelection()
    {

        // fires ray from the camera to see if we clicked on the unit
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = LayerMask.GetMask("Units");
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            if (hit.transform.TryGetComponent<Unit>(out Unit unit) && !unit.IsEnemy())
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        // invokes event if we selected other unit and selects it
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool CanSteamMachineMove()
    {
        if (ResourceManager.Instance.CoalCount >= selectedUnit.GetMovementCost()) return true;
        else return false;
    }
    private Zone GetClickedZone(Vector3 mouseWorldPosition)
    {
        // gets clicked zone
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
