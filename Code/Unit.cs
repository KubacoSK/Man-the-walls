using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private Vector2 targetPosition;

    private void Update()
    {
        float moveSpeed = 4f;
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);

        if (Input.GetKeyDown(KeyCode.T))
        {
            Move(new Vector2(4, 4));
        }
    }
    private void Move(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
    }
}
