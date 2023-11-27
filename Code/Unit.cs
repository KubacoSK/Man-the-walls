using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

    private Vector2 targetPosition;
    private void Awake()
    {
        targetPosition = transform.position;
    }

    private void Update()
    {
        float moveSpeed = 4f;
        // defines move speed

        float step = moveSpeed * Time.deltaTime;
        // makes it framerate independent

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
        //moves towards position
    }
    public void Move(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
        // sets target position

    }
}
