using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{

    public static Vector2 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // fires a ray from main camera towards the mouse position

        int layerMask = LayerMask.GetMask("GridPoints");
        // makes it that the ray only reacts to certain objects based on layer they are on

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);
        // detects if the thing hit by ray is an object or an empty space
        
        return hit.point;
        // returns the position the ray hits
    }
}
