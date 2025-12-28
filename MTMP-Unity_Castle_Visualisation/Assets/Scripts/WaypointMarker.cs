using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[ExecuteAlways] // allows marker updates also in edit mode of Unity
public class WaypointMarker : MonoBehaviour
{
    public ShowroomManager sm; // reference to manager holding json data
    public Transform target; // world-space target the marker follows
    // ui elements of the marker
    public Text label;
    public Image icon;
    public string labelText;
    public bool hideWhenOffscreen = true;
    // cached references
    private Camera cam;
    private RectTransform rect;
    private Canvas canvas;
    public bool smoothMovement = true;
    public float smoothSpeed = 10f;
    // key used to fetch text from dictionary made from json data
    public string jsonKey;
    // info panel shown after clicking marker
    public GameObject infoPanel;
    public Text infoPanelTitle;
    public Text infoPanelText;
    // internal positions for interpolation
    private Vector2 currentAnchoredPos;
    private Vector2 targetAnchoredPos;

    private void Awake()
    {
        Init(); // initializes camera and canvas
        ApplyLabelText(); // applies marker text
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // keeps preview updated in inspector
        ApplyLabelText();
        Init();
    }
#endif

    private void Init()
    {
        // caches rect and parent canvas
        rect = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        // selects correct camera based on canvas mode
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera && canvas.worldCamera != null)
        {
            cam = canvas.worldCamera;
        }
        else
        {
            cam = Camera.main;
        }
    }

    private void LateUpdate()
    {
        if (target == null || cam == null || canvas == null)
            return;
        // converts target position to screen space
        Vector3 screenPos = cam.WorldToScreenPoint(target.position);
        // checks if target is inside camera view
        bool isVisible = screenPos.z > 0.01f && screenPos.x >= 0 && screenPos.x <= cam.pixelWidth && screenPos.y >= 0 && screenPos.y <= cam.pixelHeight;

        if (hideWhenOffscreen)
            SetVisible(isVisible);
        // converts screen position to canvas local position
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, screenPos, cam, out targetAnchoredPos);
        // applies instant or smooth movement
        if (!smoothMovement)
        {
            rect.anchoredPosition = targetAnchoredPos;
            currentAnchoredPos = targetAnchoredPos;
        }
        else
        {
            currentAnchoredPos = Vector2.Lerp(rect.anchoredPosition, targetAnchoredPos, Time.deltaTime * smoothSpeed);
            rect.anchoredPosition = currentAnchoredPos;
        }
    }

    private void SetVisible(bool visible) // enables or disables marker ui
    {
        if (label != null)
            label.enabled = visible;
        if (icon != null)
            icon.enabled = visible;
    }


    private void ApplyLabelText() // updates marker label text
    {
        if (label != null)
            label.text = labelText;
    }
    public void SetTarget(Transform newTarget) => target = newTarget; // assigns a new target to follow

    public void SetText(string text) // sets marker text at runtime
    {
        labelText = text;
        ApplyLabelText();
    }
    public void clickMarker()  // called when marker is clicked - show object information from json dictionary
    {
        infoPanelTitle.text = labelText;
        if (sm.data.TryGetValue(jsonKey, out string value)) // loads text from dictionary made from json and shows panel
        {
            infoPanelText.text = value;
            infoPanel.SetActive(true);
        }
    }
}
