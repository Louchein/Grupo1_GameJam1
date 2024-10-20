using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    float xRotation = 0;
    public float mouseSensitivity = 80f;
    public float anguloCam;
    float mouseX, mouseY;

    public Transform player;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Bloquear el cursor en el centro de la pantalla
    }

    // Update is called once per frame
    void Update()
    {
        InputManager();
        RotateCam();
    }


    void InputManager()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
    }

    void RotateCam()
    {
        //xRotation -= mouseY;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -anguloCam, anguloCam);
        //transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.localRotation = Quaternion.Euler(xRotation, 180, 0);
        player.Rotate(Vector3.up * mouseX);
    }
}
