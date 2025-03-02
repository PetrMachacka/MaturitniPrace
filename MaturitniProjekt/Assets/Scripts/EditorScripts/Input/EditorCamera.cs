using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorCamera : MonoBehaviour
{

    public Transform orientation;
    public float lookSpeed = 200f;
    float xRotation;
    float yRotation;
    private void Start()
    {
        lookSpeed = lookSpeed * PlayerPrefs.GetFloat("Sensitivity");
    }
    private void Update()
    {
        if(!PauseMenu.isPaused){
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * lookSpeed * 5;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * lookSpeed * 5;

            yRotation += mouseX;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
            }
            else{
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
    }
}
