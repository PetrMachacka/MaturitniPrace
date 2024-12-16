using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour, EditorInputSystem.IEditorActions
{
    private EditorInputSystem inputSystem;

    private void Awake()
    {
        inputSystem = new EditorInputSystem();
        inputSystem.Editor.SetCallbacks(this);
    }

    private void OnEnable()
    {
        inputSystem.Editor.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Editor.Disable();
    }

    public void OnNumberKeys(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            int key = int.Parse(context.control.name);
            if(key > 0){
                key--;
            }
            else if (key == 0)
            {
                key = 9;
            }
            Debug.Log($"Number key pressed: {key}");
        }
    }

    public void OnMovement(InputAction.CallbackContext context) { }
    public void OnLeftClick(InputAction.CallbackContext context) { }
    public void OnRightClick(InputAction.CallbackContext context) { }
    public void OnESCAPE(InputAction.CallbackContext context) { }
    public void OnPhoto(InputAction.CallbackContext context) { }
}