using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    
    [SerializeField] private int maxMoveDistance = 1;

    private Vector2 targetPosition;
    private Unit unit;
    private void Awake()
    {
        unit = GetComponent<Unit>();
        targetPosition = transform.position;
    }

    private void Update()
    {
        float moveSpeed = 4f;
        // defines move speed
        float step = moveSpeed * Time.deltaTime;
        // makes it framerate independent
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, step);
    }
    public void Move(Vector2 targetPosition)
    {
        this.targetPosition = targetPosition;
        // sets target position
    }

    public List<Zone> GetValidZonesList()
    {
        List<Zone> validZoneList = new List<Zone>();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, maxMoveDistance);

        foreach (var collider in colliders)
        {
            Zone zone = collider.GetComponent<Zone>();
            if (zone != null)
            {
                validZoneList.Add(zone);
            }
        }
        foreach (var zone in validZoneList)
        {
            Debug.Log(zone.name);
        }

        return validZoneList;
    }
}
