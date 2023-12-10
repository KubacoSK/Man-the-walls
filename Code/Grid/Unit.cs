using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private MoveAction moveAction;
    private void Awake()
    { 
        moveAction = GetComponent<MoveAction>();
    }

    private void Update()
    {
        
    }
    public MoveAction GetMoveAction()
    {
        return moveAction;
    }
    private bool IsOnZone()
    {
        // Get the current position of the unit
        Vector2 unitPosition = transform.position;

        // Perform overlap check with the "Zone" layer (you can customize the layer name)
        Collider2D hitCollider = Physics2D.OverlapPoint(unitPosition, LayerMask.GetMask("GridPoints"));

        // Check if a collider was hit
        return hitCollider != null;
    }

}
