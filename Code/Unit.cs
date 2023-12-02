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

}
