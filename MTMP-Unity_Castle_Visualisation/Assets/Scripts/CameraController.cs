using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Controller")]
    public bool unlockedCamera = false;
    [Header("Zoom")]
    public float zoomSpeed = 10f;
    public float minZoom = 5f;
    public float maxZoom = 40f;
    [Header("Movement")]
    public float moveSpeed = 10f;
    public float moveShiftBoost = 2f;
    public float smoothing = 10f;
    public float mouseUpSpeed = 1f;
    public float rotationSpeed = 3f;
    private float targetZoom;
    private Vector3 targetPosition;
    private Vector3 lastMousePosition;
    [Header("Move to info target logic")]
    private Transform moveTarget = null;
    public float moveToSpeed = 50f;
    public float rotateToSpeed = 250f;
    private Transform lastTarget;
    public GameObject[] targetControlElements;
    public GameObject lockButton;
    public GameObject unlockButton;
    void Start()
    {
        if (Camera.main != null)
            targetZoom = Camera.main.fieldOfView;

        targetPosition = transform.position;
        lastTarget = transform;
        lockButton.SetActive(false);
        unlockButton.SetActive(false);
    }

    void Update()
    {
        HandleAutoCameraMove();
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetZoom, Time.deltaTime * smoothing);
        if (unlockedCamera)
        {
            HandleMouse();
            HandleMovement();
            HandleRotation();
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothing);
        }
    }

    private void HandleMouse()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
            targetZoom -= scroll * zoomSpeed;

        if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.KeypadPlus))
            targetZoom -= zoomSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
            targetZoom += zoomSpeed * Time.deltaTime;

        targetZoom = Mathf.Clamp(targetZoom, minZoom, maxZoom);

        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetZoom, Time.deltaTime * smoothing);

        if (Input.GetMouseButtonDown(0))
            lastMousePosition = Input.mousePosition;

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
        float speed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed *= moveShiftBoost;

        Vector3 input = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
        );

        if (input.sqrMagnitude > 0.01f)
        {
            Vector3 moveDirection = transform.forward * input.z + transform.right * input.x;
            targetPosition += moveDirection.normalized * speed * Time.deltaTime;
        }
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
        if (!Input.GetMouseButton(1))
            return;
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed;
        transform.Rotate(Vector3.up, mouseX, Space.World);
        transform.Rotate(Vector3.right, -mouseY, Space.Self);
    }

    public void StartCameraMove(GameObject target)
    {
        Transform targetTransform = target.transform;
        moveTarget = targetTransform;
        lastTarget = targetTransform;
        unlockButton.SetActive(true);
    }

    private void HandleAutoCameraMove()
    {
        if (moveTarget == null)
            return;
        transform.position = Vector3.MoveTowards(
            transform.position,
            moveTarget.position,
            moveToSpeed * Time.deltaTime
        );

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            moveTarget.rotation,
            rotateToSpeed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, moveTarget.position) < 0.01f &&
            Quaternion.Angle(transform.rotation, moveTarget.rotation) < 0.5f)
        {
            transform.position = moveTarget.position;
            transform.rotation = moveTarget.rotation;

            moveTarget = null;
            targetPosition = transform.position;
        }
    }

    public void ManageCameraLock(bool lockStatus)
    {
        unlockedCamera = lockStatus;
        if (unlockedCamera == false)
        {
            targetZoom = 60f;
            StartCameraMove(lastTarget.gameObject);
            unlockButton.SetActive(true);
            lockButton.SetActive(false);
            foreach (GameObject targetController in targetControlElements)
            {
                targetController.SetActive(true);
            }
        } else
        {
            targetPosition = transform.position;
            targetZoom = Camera.main.fieldOfView;
            unlockButton.SetActive(false);
            lockButton.SetActive(true);
            foreach (GameObject targetController in targetControlElements)
            {
                targetController.SetActive(false);
            }
        }
            
    }

}
