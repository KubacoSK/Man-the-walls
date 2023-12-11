using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float panSpeed = 10f;
    private float panBorderThickness = 10f;
    public Vector2 panLimit;
    public Vector2 panMinimum;
    public float minOrthographicSize = 2f;
    public float maxOrthographicSize = 10f;
    public float zoomSmoothness = 5f;
    private float targetOrthographicSize;

    public float scrollSpeed;


    private void Update()
    {
        CameraMovement();
        CameraZoom();
    }

    private void CameraMovement()
    {
        Vector3 Pos = transform.position;
        // moves camera by comparing screenheight to border
        if (Input.GetKey("w") || Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            Pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("s") || Input.mousePosition.y <= panBorderThickness)
        {
            Pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("a") || Input.mousePosition.x <= panBorderThickness)
        {
            Pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey("d") || Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            Pos.x += panSpeed * Time.deltaTime;
        }


        Pos.x = Mathf.Clamp(Pos.x, panMinimum.x, panLimit.x);
        Pos.y = Mathf.Clamp(Pos.y, panMinimum.y, panLimit.y);
        transform.position = Pos;

    }
    private void CameraZoom()
    {

        Vector3 Pos = transform.position;
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetOrthographicSize = Mathf.Clamp(Camera.main.orthographicSize - scroll * scrollSpeed * 100f * Time.deltaTime, minOrthographicSize, maxOrthographicSize);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSmoothness);
    }
}