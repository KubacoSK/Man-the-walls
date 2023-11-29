using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineFreeLook freeLookCamera;

    void Update()
    {
        // Pan with arrow keys or WASD
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        freeLookCamera.m_XAxis.Value += horizontal;
        freeLookCamera.m_YAxis.Value += vertical;

        // Zoom with the mouse wheel
        float zoom = Input.GetAxis("Mouse ScrollWheel");
        freeLookCamera.m_Orbits[0].m_Radius += zoom * 5f; // Adjust the multiplier as needed
    }
}