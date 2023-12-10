using System;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionsSystem : MonoBehaviour
{
    public static UnitActionsSystem Instance { get; private set; }

    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] private Unit selectedUnit;

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
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;

            // Right-click to move the selected unit
            Vector3 mouseWorldPosition = MouseWorld.GetPosition();
            Zone clickedZone = GetClickedZone(mouseWorldPosition);

            if (clickedZone != null)
            {
                List<Zone> validZones = selectedUnit.GetMoveAction().GetValidZonesList();

                if (IsValidClickedZone(clickedZone, validZones))
                {
                    Vector2 centerPosition = clickedZone.transform.position; // Assuming the zone's center is the desired position

                    if (selectedUnit != null)
                    {
                        selectedUnit.GetMoveAction().Move(centerPosition);
                    }
                }
            }
        }
    }

    private bool IsValidClickedZone(Zone clickedZone, List<Zone> validZones)
    {
        return validZones.Contains(clickedZone);
    }

    private bool TryHandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = LayerMask.GetMask("Units");
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);

        if (hit.collider != null)
        {
            if (hit.transform.TryGetComponent<Unit>(out Unit unit))
            {
                SetSelectedUnit(unit);
                return true;
            }
        }
        return false;
    }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private Zone GetClickedZone(Vector3 mouseWorldPosition)
    {
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
