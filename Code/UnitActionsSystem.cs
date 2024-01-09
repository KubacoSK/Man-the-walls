using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
            switch (selectedUnit.GetHorse())
            {

                case true:

                    // checks if we right clicked, unit isnt moving and selected unit has enough movement points
                    if (Input.GetMouseButtonDown(1) && IsMoving == false && selectedUnit.GetActionPoints() < 3 && selectedUnit != null)
                    {
                        Debug.Log("horse go go");
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
                                float x = 0;
                                float y = 0;
                                // moves unit on x and y axis depending on number of units inside the zone
                                foreach (Unit unitinzone in UnitsInZone)
                                {
                                    x -= 0.4f;
                                    if (x < -0.8f)
                                    {
                                        y += -0.8f;
                                        x = 0;
                                    }
                                }
                                if (clickedZone.GetZoneSizeModifier().x == 1) y += 0.4f;

                                // gets center position of the clicked zone
                                centerPosition = clickedZone.transform.position;
                                centerPosition.x += x;
                                centerPosition.y += y;
                                if (selectedUnit != null)
                                {
                                    // moves to to position
                                    IsMoving = true;
                                    selectedUnit.GetMoveAction().Move(centerPosition);
                                    selectedUnit.DoAction(clickedZone);
                                }
                            }
                        }
                    }
                    break;

                case false:

                    if (Input.GetMouseButtonDown(1) && IsMoving == false && selectedUnit.GetActionPoints() < 2 && selectedUnit != null)
                    {

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
                                float x = 0;
                                float y = 0;
                                // moves unit on x and y axis depending on number of units inside the zone
                                foreach (Unit unitinzone in UnitsInZone)
                                {
                                    x -= 0.4f;
                                    if (x < -0.8f)
                                    {
                                        y += -0.8f;
                                        x = 0;
                                    }
                                }
                                if (clickedZone.GetZoneSizeModifier().x == 1) y += 0.4f;

                                // gets center position of the clicked zone
                                centerPosition = clickedZone.transform.position;
                                centerPosition.x += x;
                                centerPosition.y += y;
                                if (selectedUnit != null)
                                {
                                    // moves to to position
                                    IsMoving = true;
                                    selectedUnit.GetMoveAction().Move(centerPosition);
                                    selectedUnit.DoAction(clickedZone);
                                }
                            }
                        }
                    }
                    break;
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
