using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    private float panSpeed = 0.6f;
    public Vector2 panLimit;
    public Vector2 panMinimum;
    public float minOrthographicSize = 2f;
    public float maxOrthographicSize = 10f;
    public float zoomSmoothness = 5f;
    private float targetOrthographicSize;
    public float scrollSpeed = 5;
    public float CameraSize;
    public static event EventHandler CameraSizeChanged;

    private void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("Existuje viac ako jeden CameraController! " + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Update()
    {
        CameraMovement();
        CameraZoom();
    }

    private void CameraMovement()
    {
        Vector3 Pos = transform.position;

        // Dotykové ovládanie pre pohyb kamery
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                Pos.x -= touch.deltaPosition.x * panSpeed * Time.deltaTime;
                Pos.y -= touch.deltaPosition.y * panSpeed * Time.deltaTime;
            }
        }

        // Obmedzenie pozície kamery na definované limity
        Pos.x = Mathf.Clamp(Pos.x, panMinimum.x + Camera.main.orthographicSize, panLimit.x - Camera.main.orthographicSize);
        // Pos y meníme trochu menej, pretože používame režim na šírku
        Pos.y = Mathf.Clamp(Pos.y, panMinimum.y + Camera.main.orthographicSize / 2, panLimit.y - Camera.main.orthographicSize / 2);

        transform.position = Pos;
    }

    private void CameraZoom()
    {
        float scroll = 0;

        // Dotykové ovládanie pre priblíženie kamery (gesto zvierania prstov)
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevMagnitude = (touch0PrevPos - touch1PrevPos).magnitude;
            float currentMagnitude = (touch0.position - touch1.position).magnitude;

            scroll = prevMagnitude - currentMagnitude; // Vypočíta množstvo priblíženia
            scroll *= 0.1f;  // rýchlosť zoomu
        }

        // Aktualizácia ortografickej veľkosti kamery
        targetOrthographicSize = Mathf.Clamp(Camera.main.orthographicSize + scroll * scrollSpeed * Time.deltaTime, minOrthographicSize, maxOrthographicSize);
        Camera.main.orthographicSize = targetOrthographicSize;

        CameraSize = Camera.main.orthographicSize;
        CameraSizeChanged?.Invoke(this, EventArgs.Empty);
    }
}
