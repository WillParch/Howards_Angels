using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerDirection : MonoBehaviour
{
    public Transform cameraTransform;
    public float turnSpeed = 10f;

    private CharacterController controller;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Calculate the direction the controller should face based on the camera's rotation
        Vector3 forward = cameraTransform.forward;
        forward.y = 0f;
        forward.Normalize();

        // Rotate the controller gradually to face the desired direction
        Vector3 currentForward = transform.forward;
        currentForward.y = 0f;
        currentForward = Vector3.RotateTowards(currentForward, forward, turnSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(currentForward);
    }
}
