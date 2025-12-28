using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool unlockedCamera = false;
    // zoom settings
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 100f;
    // movement settings
    public float moveSpeed = 10f;
    public float moveShiftBoost = 2f;
    public float smoothing = 10f;
    public float mouseUpSpeed = 1f;
    public float rotationSpeed = 3f;

    private float targetZoom; // target field of view used for smooth zooming
    private Vector3 targetPosition; // target position used for smooth movement
    private Vector3 lastMousePosition; // stores previous mouse position for drag calculations
    public float defaultFOV = 60f;
    public Vector3 cameraOffset = new Vector3(0, 5, -10);   // optional offset for automatic camera positioning

    private void Awake()
    {
        if (Camera.main != null)
        {
            Camera.main.fieldOfView = defaultFOV;
            targetZoom = defaultFOV;
        }
        targetPosition = transform.position;
    }

    void Update()
    {
        if (unlockedCamera && Camera.main != null)
        {
            // smoothly interpolate the camera field of view
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetZoom, Time.deltaTime * smoothing);
            HandleMouse(); // handle zoom and vertical mouse drag
            HandleMovement(); // handle keyboard-based movement
            HandleRotation(); // handle mouse-based rotation
            // smoothly interpolate camera position
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothing);
        }
    }


    private void HandleMouse()
    {
        // mouse wheel zoom input
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
            targetZoom -= scroll * zoomSpeed;
        // keyboard zoom input (+ / -)
        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus))
            targetZoom -= zoomSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
            targetZoom += zoomSpeed * Time.deltaTime;
        // clamp zoom to defined limits
        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetZoom, Time.deltaTime * smoothing);
        // store mouse position when dragging starts
        if (Input.GetMouseButtonDown(0))
            lastMousePosition = Input.mousePosition;
        // vertical camera movement using left mouse drag
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;
            lastMousePosition = Input.mousePosition;
            Vector3 up = transform.up;
            up.z = 0;
            up.Normalize();
            Vector3 move = delta.y * mouseUpSpeed * Time.deltaTime * -up;
            targetPosition += move;
        }
    }
    private void HandleMovement()
    {
        // choose movement speed - shift - faster movement
        float speed = moveSpeed;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= moveShiftBoost;

        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        // move camera relative to its orientation
        if (input.sqrMagnitude > 0.01f)
        {
            Vector3 moveDirection = transform.forward * input.z + transform.right * input.x;
            targetPosition += moveDirection.normalized * speed * Time.deltaTime;
        }
        // vertical movement using Space key
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 up = transform.up;
            up.z = 0;
            up.Normalize();
            targetPosition += moveSpeed * Time.deltaTime * up;
        }
    }
    private void HandleRotation()
    {
        // rotate camera only using right mouse button
        if (!Input.GetMouseButton(1))
            return;
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        transform.Rotate(Vector3.up, mouseX, Space.World); // horizontal rotation around world Y-axis
        transform.Rotate(Vector3.right, -mouseY, Space.Self); // vertical rotation around local X-axis
    }
}
