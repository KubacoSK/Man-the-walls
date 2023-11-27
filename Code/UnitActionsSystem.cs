using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitActionsSystem : MonoBehaviour
{
    [SerializeField] private Unit selectedUnit;

    private void Update()
    {
        if(TryHandleUnitSelection()) return;

        if(Input.GetMouseButtonDown(0))
        {
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
                selectedUnit = unit;
                return true;
            }

        }
        return false;
    }
}

