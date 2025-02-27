
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;

    public void Awake()
    {
        instance = this;
    }
    public static Vector2 GetPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // vystreli ray z hlavnej kamery smerom k pozícii myši

        int layerMask = LayerMask.GetMask("GridPoints");
        // zabezpečí, že ray reaguje iba na určité objekty na základe ich vrstvy

        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);
        // detekuje, či ray zasiahol objekt alebo prázdne miesto

        return hit.point;
        // vráti pozíciu, kde ray zasiahne
    }
}
