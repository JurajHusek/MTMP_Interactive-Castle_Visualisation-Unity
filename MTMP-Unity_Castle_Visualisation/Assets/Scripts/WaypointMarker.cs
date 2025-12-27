using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class WaypointMarker : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("UI")]
    [SerializeField] private Text label;
    [SerializeField] private Image icon;


    [Header("Text")]
    [SerializeField] private string labelText;

    [Header("Settings")]
    [SerializeField] private Vector3 worldOffset = Vector3.up;
    [SerializeField] private bool hideWhenOffscreen = true;

    private Camera cam;
    private RectTransform rect;
    private Canvas canvas;
    [Header("Smoothing")]
    [SerializeField] private bool smoothMovement = true;
    [SerializeField] private float smoothSpeed = 10f;

    private Vector2 currentAnchoredPos;
    private Vector2 targetAnchoredPos;

    private void Awake()
    {
        Init();
        ApplyLabelText();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        ApplyLabelText();
        Init();
    }
#endif

    private void Init()
    {
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("WaypointMarker must be a child of a Canvas!", this);
            return;
        }

        cam = canvas.renderMode == RenderMode.ScreenSpaceCamera
            ? canvas.worldCamera
            : Camera.main;

        if (label == null)
            Debug.LogWarning("WaypointMarker: Label not assigned", this);
    }

    private void LateUpdate()
    {
        if (target == null || cam == null || canvas == null)
            return;

        Vector3 screenPos = cam.WorldToScreenPoint(target.position);

        bool isVisible =
            screenPos.z > 0.01f &&
            screenPos.x >= 0 && screenPos.x <= cam.pixelWidth &&
            screenPos.y >= 0 && screenPos.y <= cam.pixelHeight;

        if (hideWhenOffscreen)
            SetVisible(isVisible);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            screenPos,
            cam,
            out targetAnchoredPos
        );

        if (!smoothMovement)
        {
            rect.anchoredPosition = targetAnchoredPos;
            currentAnchoredPos = targetAnchoredPos;
        }
        else
        {
            currentAnchoredPos = Vector2.Lerp(
                rect.anchoredPosition,
                targetAnchoredPos,
                Time.deltaTime * smoothSpeed
            );

            rect.anchoredPosition = currentAnchoredPos;
        }
    }


    private void SetVisible(bool visible)
    {
        if (label != null)
            label.enabled = visible;
        if (icon != null)
            icon.enabled = visible;
    }


    private void ApplyLabelText()
    {
        if (label != null)
            label.text = labelText;
    }

    // --- Public API (volite¾né) ---
    public void SetTarget(Transform newTarget) => target = newTarget;

    public void SetText(string text)
    {
        labelText = text;
        ApplyLabelText();
    }
}
