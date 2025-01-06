using UnityEngine;
using UnityEngine.InputSystem;

public class FPSController : MonoBehaviour
{
    [SerializeField] private float walkingSpeed = 5f;
    [SerializeField] private float runningSpeed = 10f;
    [SerializeField] private float jumpForce = 8f;
    public Camera playerCamera;
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;
    public bool isController = false;
    private Rigidbody rb;
    private EditorInputSystem editorInputSystem;
    private Vector3 movementInput;
    private Vector2 cameraInput;
    private float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        editorInputSystem = new EditorInputSystem();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Handle camera rotation
        if(isController){
            rotationX += -cameraInput.y * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, cameraInput.x * lookSpeed, 0);
        }
        else{
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * lookSpeed * 5;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * lookSpeed * 5;

            rotationX -= mouseY;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, mouseX, 0);
        }

    }

    void FixedUpdate()
    {
        if (canMove)
        {
            Vector3 forwardMovement = transform.forward * movementInput.z;
            Vector3 rightMovement = transform.right * movementInput.x;
            Vector3 movement = (forwardMovement + rightMovement) * (Input.GetKey(KeyCode.LeftShift) ? runningSpeed : walkingSpeed);

            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.z);

            if (movementInput.y > 0 && IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }

    private void OnMovement(InputValue inputValue)
    {
        Vector3 input = inputValue.Get<Vector3>();
        movementInput.x = input.x;
        movementInput.z = input.z;
        movementInput.y = input.y;
    }

    private void OnCamera(InputValue inputValue)
    {
        cameraInput = inputValue.Get<Vector2>();
    }
    
    private void OnESCAPE(){
        Errors.ShowError("Are you sure you want to exit?");
        if(Cursor.lockState == CursorLockMode.Locked){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else{
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}