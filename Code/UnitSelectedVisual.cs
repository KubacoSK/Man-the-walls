using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private SpriteRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        UnitActionsSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        
        UpdateVisual();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs Empty)
    {
        UpdateVisual();

    }
    private void UpdateVisual()
    {
        if (UnitActionsSystem.Instance.GetSelectedUnit() == unit)
        {
            meshRenderer.enabled = true;
        }
        else
        {
            meshRenderer.enabled = false;
        }
    }
}
