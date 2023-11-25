using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        int layerMask = LayerMask.GetMask("GridPoints");
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);
        if (hit.collider != null)
        {
            // Log information about the hit
            Debug.Log($"Hit object: {hit.collider.gameObject.name}, at point: {hit.point}");
        }
        else
        {
            // Log that the ray did not hit anything
            Debug.Log("Ray did not hit any object.");
        }
    }
}
