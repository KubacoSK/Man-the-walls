using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        

        if(Input.GetMouseButtonDown(0))
        {
            if(TryHandleUnitSelection()) return;
            // Right-click to move the selected unit
            selectedUnit.Move(MouseWorld.GetPosition());
        }
    }
    private bool TryHandleUnitSelection()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // fires a ray from main camera towards the mouse position

        int layerMask = LayerMask.GetMask("Units");
        // makes it that the ray only reacts to certain objects based on layer they are on

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);
        // detects if the thing hit by ray is an object or an empty space 

        if(hit.collider != null)
        {
            // check if selected unit has unit component
            if(hit.transform.TryGetComponent<Unit>(out Unit unit))
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

    public Unit GetSelectedUnit()
    {
        return selectedUnit;
    }
}

