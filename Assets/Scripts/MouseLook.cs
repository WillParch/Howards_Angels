using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform player;
    public Transform projectileSpawn;
    public float mouseSensitivity = 100f;

    private float xRotation = 0f;

    void Start()
    {
       // Cursor.lockState = CursorLockMode.Locked;
    }

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);

        // Rotate projectile spawn along the x and y axes
        projectileSpawn.rotation = Quaternion.Euler(transform.eulerAngles.x, player.eulerAngles.y, transform.eulerAngles.z);
    }
}

