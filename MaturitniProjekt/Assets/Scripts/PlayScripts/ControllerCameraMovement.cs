using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerCameraMovement : MonoBehaviour
{
    [SerializeField] private float lookSpeed = 2.0f;
    [SerializeField] private float lookXLimit = 45.0f;

    private EditorInputSystem editorInputSystem;
    private Vector2 cameraInput;
    private float rotationX = 0;

    private void Awake()
    {
        editorInputSystem = new EditorInputSystem();
    }

    private void OnEnable()
    {
        editorInputSystem.CharacterController.Enable();
        editorInputSystem.CharacterController.CameraControl.performed += OnCameraControl;
        editorInputSystem.CharacterController.CameraControl.canceled += OnCameraControl;
    }

    private void OnDisable()
    {
        editorInputSystem.CharacterController.CameraControl.performed -= OnCameraControl;
        editorInputSystem.CharacterController.CameraControl.canceled -= OnCameraControl;
        editorInputSystem.CharacterController.Disable();
    }

    private void OnCameraControl(InputAction.CallbackContext context)
    {
        cameraInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        // Handle camera rotation
        rotationX += -cameraInput.y * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.parent.rotation *= Quaternion.Euler(0, cameraInput.x * lookSpeed, 0);
    }
}