using UnityEngine;
using UnityEngine.EventSystems;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

[RequireComponent(typeof(Camera))]
public class PanZoom2D : MonoBehaviour
{
    private Camera cam;

    [Header("Zoom (Orthographic)")]
    [Tooltip("Small numbers zoom in; large numbers zoom out.")]
    public float minOrthoSize = 2f;
    public float maxOrthoSize = 20f;
    [Tooltip("How much to change size per scroll tick.")]
    public float zoomStep = 1.0f;
    [Tooltip("Smooth the zoom each frame.")]
    public bool smoothZoom = true;
    [Tooltip("How fast to smooth toward target size.")]
    public float zoomLerpSpeed = 15f;

    [Header("Pan (Middle Mouse Drag)")]
    [Tooltip("Scales drag movement. Higher = faster pan.")]
    public float panSpeed = 1.0f;
    [Tooltip("Scale pan by zoom so it feels consistent at all zoom levels.")]
    public bool panScalesWithZoom = true;

    [Header("Optional Bounds")]
    [Tooltip("If set, camera will be clamped so the view stays inside this collider's bounds.")]
    public Collider2D boundsCollider;
    [Tooltip("Additional padding inside bounds (world units).")]
    public float boundsPadding = 0.0f;

    // Runtime
    private float targetOrthoSize;
    private bool dragging;
    private Vector3 dragWorldAnchor;
    private Vector3 camPosOnDragStart;

    void Awake()
    {
        cam = GetComponent<Camera>();
        cam.orthographic = true; // this script is for 2D ortho
        targetOrthoSize = Mathf.Clamp(cam.orthographicSize, minOrthoSize, maxOrthoSize);
    }

    void Update()
    {
        if (IsPointerOverUI()) return; // ignore when over UI

        HandleZoom();
        HandlePan();
    }

    // -------- Zoom --------
    void HandleZoom()
    {
        float scrollY = GetScrollY();
        if (Mathf.Abs(scrollY) > 0.0001f)
        {
            // Zoom toward the mouse position (keeps the pointed spot stable)
            Vector3 mouseWorldBefore = cam.ScreenToWorldPoint(GetMouseScreenPos());

            targetOrthoSize = Mathf.Clamp(
                targetOrthoSize - scrollY * zoomStep,
                minOrthoSize, maxOrthoSize
            );

            float newSize = smoothZoom
                ? Mathf.Lerp(cam.orthographicSize, targetOrthoSize, Time.unscaledDeltaTime * zoomLerpSpeed)
                : targetOrthoSize;

            cam.orthographicSize = newSize;

            Vector3 mouseWorldAfter = cam.ScreenToWorldPoint(GetMouseScreenPos());
            Vector3 delta = mouseWorldBefore - mouseWorldAfter;
            transform.position = ClampToBounds(transform.position + delta);
        }
        else if (smoothZoom && !Mathf.Approximately(cam.orthographicSize, targetOrthoSize))
        {
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetOrthoSize, Time.unscaledDeltaTime * zoomLerpSpeed);
            transform.position = ClampToBounds(transform.position); // keep clamped while zooming
        }
    }

    // -------- Pan (MMB drag) --------
    void HandlePan()
    {
        if (IsMMBDown())
        {
            dragging = true;
            dragWorldAnchor = cam.ScreenToWorldPoint(GetMouseScreenPos());
            camPosOnDragStart = transform.position;
        }

        if (dragging && IsMMBHeld())
        {
            Vector3 currentWorld = cam.ScreenToWorldPoint(GetMouseScreenPos());
            Vector3 worldDelta = dragWorldAnchor - currentWorld;

            float scale = panScalesWithZoom ? cam.orthographicSize : 1f;
            Vector3 target = camPosOnDragStart + worldDelta * panSpeed * scale;

            transform.position = ClampToBounds(target);
        }

        if (dragging && IsMMBUp())
        {
            dragging = false;
        }
    }

    // -------- Bounds clamp --------
    Vector3 ClampToBounds(Vector3 desired)
    {
        if (boundsCollider == null) return desired;

        Bounds b = boundsCollider.bounds;
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        float minX = b.min.x + halfW + boundsPadding;
        float maxX = b.max.x - halfW - boundsPadding;
        float minY = b.min.y + halfH + boundsPadding;
        float maxY = b.max.y - halfH - boundsPadding;

        // If bounds are smaller than view, just lock to center.
        if (minX > maxX) { minX = maxX = b.center.x; }
        if (minY > maxY) { minY = maxY = b.center.y; }

        desired.x = Mathf.Clamp(desired.x, minX, maxX);
        desired.y = Mathf.Clamp(desired.y, minY, maxY);
        desired.z = transform.position.z; // preserve camera Z

        return desired;
    }

    // -------- Helpers: Input (supports new and old systems) --------
    static Vector3 GetMouseScreenPos()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null ? (Vector3)Mouse.current.position.ReadValue() : Input.mousePosition;
#else
        return Input.mousePosition;
#endif
    }

    static bool IsMMBDown()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.middleButton.wasPressedThisFrame;
#else
        return Input.GetMouseButtonDown(2);
#endif
    }

    static bool IsMMBHeld()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.middleButton.isPressed;
#else
        return Input.GetMouseButton(2);
#endif
    }

    static bool IsMMBUp()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null && Mouse.current.middleButton.wasReleasedThisFrame;
#else
        return Input.GetMouseButtonUp(2);
#endif
    }

    static float GetScrollY()
    {
#if ENABLE_INPUT_SYSTEM
        return Mouse.current != null ? Mouse.current.scroll.ReadValue().y / 120f : Input.mouseScrollDelta.y;
#else
        // On many mice, scroll steps come in multiples of 120 units; keep it as-is
        return Input.mouseScrollDelta.y;
#endif
    }

    static bool IsPointerOverUI()
    {
        // If there is no EventSystem, assume not over UI
        if (EventSystem.current == null) return false;
#if ENABLE_INPUT_SYSTEM
        // For the new system, IsPointerOverGameObject() without id works in most cases
        return EventSystem.current.IsPointerOverGameObject();
#else
        // Legacy: 0 is the mouse pointer id
        return EventSystem.current.IsPointerOverGameObject();
#endif
    }
}
