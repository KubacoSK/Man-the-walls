using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool isUnitSelected = false;
    private Vector2 targetPosition;

    void Update()
    {
        // Check for unit selection
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Unit"))
            {
                isUnitSelected = true;
            }
            else if (isUnitSelected)
            {
                // Set the target position to the clicked point
                SetTargetPosition(clickPosition);
            }
        }

        // Move towards the target position if the unit is selected
        if (isUnitSelected)
        {
            MoveToTargetPosition();
        }
    }

    void SetTargetPosition(Vector2 position)
    {
        targetPosition = position;
    }

    void MoveToTargetPosition()
    {
        // Move the unit towards the target position
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        // Check if the unit has reached the target position
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            isUnitSelected = false;
        }
    }
}