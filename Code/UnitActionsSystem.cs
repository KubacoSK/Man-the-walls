using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using UnityEngine;

public class UnitActionsSystem : MonoBehaviour
{
    public static UnitActionsSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;


    private Unit selectedUnit;
    private bool IsMoving = false;
    Vector2 centerPosition;

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
                if (Vector2.Distance(selectedUnit.transform.position, centerPosition) < 0.01f)
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
                        ResourceManager.Instance.CoalCount -= selectedUnit.GetMovementCost();
                        // Right-click to move the selected unit
                        Vector3 mouseWorldPosition = MouseWorld.GetPosition();
                        Zone clickedZone = GetClickedZone(mouseWorldPosition);

                        // checks if we clicked on zone and not on some empty space
                        if (clickedZone != null)
                        {
                            // gets list of close zones from MoveAction class
                            List<Zone> validZones = selectedUnit.GetMoveAction().GetValidZonesList();

                            if (IsValidClickedZone(clickedZone, validZones))
                            {
                                List<Unit> UnitsInZone = clickedZone.GetUnitsInZone();
                                float xOffset = 0;
                                float yOffset = 0;
                                // moves unit on x and y axis depending on number of units inside the zone
                                foreach (Unit unitinzone in UnitsInZone)
                                {
                                    xOffset -= 0.4f;
                                    if (xOffset < -0.8f)
                                    {
                                        yOffset += -0.8f;
                                        xOffset = 0;
                                    }
                                }
                                if (clickedZone.GetZoneSizeModifier().x == 1) yOffset += 0.4f;

                                // gets center position of the clicked zone
                                centerPosition = clickedZone.transform.position;
                                centerPosition.x += xOffset;
                                centerPosition.y += yOffset;
                                if (selectedUnit != null)
                                {
                                    // moves to to position
                                    IsMoving = true;
                                    selectedUnit.GetMoveAction().Move(centerPosition);
                                    selectedUnit.DoAction(clickedZone);
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
