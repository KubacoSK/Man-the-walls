using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private Vector2 targetPosition;

    private void Update()
    {
        float moveSpeed = 8f;
        // defines move speed

        float step = moveSpeed * Time.deltaTime;
        // makes it framerate independent

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
        //moves towards position

        if (Input.GetMouseButtonDown(1))
        {
            Move(MouseWorld.GetPosition());
            // by clicking right mouse button you move to designated position
        }
    }
    private void Move(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
        // sets target position

    }
}
