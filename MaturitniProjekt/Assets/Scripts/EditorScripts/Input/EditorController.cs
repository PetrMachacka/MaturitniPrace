using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EditorController : MonoBehaviour
{
    private EditorInputSystem editorInputSystem;
    private Vector3 movementInput;
    private Transform editorTransform;

    [SerializeField] private Transform orientation; 
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float verticalSpeed = 5f;

    private void Start()
    {
        editorInputSystem = new EditorInputSystem();
        editorTransform = this.transform;
    }

    private void Update()
    {
        Vector3 forwardMovement = orientation.forward * movementInput.z;
        Vector3 rightMovement = orientation.right * movementInput.x;
        Vector3 verticalMovement = Vector3.up * movementInput.y;

        Vector3 movement = (forwardMovement + rightMovement + verticalMovement).normalized;

        editorTransform.position += movement * moveSpeed * Time.deltaTime;
    }

    private void OnMovement(InputValue inputValue)
    {
        Vector3 input = inputValue.Get<Vector3>();
        movementInput.x = input.x;
        movementInput.z = input.z;

        if (input.y > 0) movementInput.y = verticalSpeed;
        else if (input.y < 0) movementInput.y = -verticalSpeed;
        else movementInput.y = 0;
    }
}