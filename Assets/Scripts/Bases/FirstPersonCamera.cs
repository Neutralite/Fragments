using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    [SerializeField]
    private Transform playerBody;

    [SerializeField]
    private float mouseSensitivity = 300f;
    private float xRotation = 0f;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
