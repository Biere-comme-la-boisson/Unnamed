using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [Header("Components")]
    public Transform player;
    public Transform cameraTransform;

    [Header("Settings")]
    public float Sensitivity = 100f;
    public float MinY = -60f;
    public float MaxY = 60f;

    private float verticalRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        verticalRotation = cameraTransform.localEulerAngles.x;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * Sensitivity * Time.deltaTime;

        player.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, MinY, MaxY);

        cameraTransform.localEulerAngles = new Vector3(verticalRotation, 0f, 0f);
    }
}
