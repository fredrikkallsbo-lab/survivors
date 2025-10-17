using Units.HealthDisplay;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CircleCollider2D))]
public class CircleFillOverlay : MonoBehaviour, IHealthDisplayer
{
    private Sprite circleSprite;
    private Color fillColor = new Color(1f, 0.35f, 0.1f, 1f);
    private float sortingOrder = 10f; 
    private float circleSize = 3;
    

    private CircleCollider2D circle;
    private Canvas canvas;
    private Image image;

    void Awake()
    {
        circle = GetComponent<CircleCollider2D>();

        // Create a world-space canvas as a child
        GameObject canvasGO = new GameObject("CircleFillCanvas");
        canvasGO.transform.SetParent(transform, false);

        canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.sortingOrder = (int)sortingOrder;

        var scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 100f; // tweak if needed

        canvasGO.AddComponent<GraphicRaycaster>();

        // Add the Image
        GameObject imgGO = new GameObject("FillImage");
        imgGO.transform.SetParent(canvasGO.transform, false);
        image = imgGO.AddComponent<Image>();
        image.sprite = circleSprite;
        image.type = Image.Type.Filled;
        image.fillMethod = Image.FillMethod.Radial360;
        image.fillOrigin = (int)Image.Origin360.Top; // change start angle if you like
        image.fillClockwise = false;
        image.color = fillColor;
        
        // Size & position it to match the collider
        UpdateGeometry();
    }

    void LateUpdate()
    {
        // If the object can scale/resize, keep it matched
        UpdateGeometry();
    }

    public void SetFill(float percent01)
    {
        image.fillAmount = Mathf.Clamp01(percent01);
    }

    private void UpdateGeometry()
    {
        float worldRadius = circle.radius * Mathf.Max(
            Mathf.Abs(transform.lossyScale.x),
            Mathf.Abs(transform.lossyScale.y)
        );
        float diameter = worldRadius * circleSize;

        // Position the canvas at the collider’s offset in world space
        Vector3 worldCenter = transform.TransformPoint(circle.offset);
        canvas.transform.position = worldCenter;
        canvas.transform.rotation = Quaternion.identity; // keep upright, or match transform.rotation if you prefer

        // Size the rect in world units (World Space Canvas uses world units)
        RectTransform rt = image.rectTransform;
        rt.sizeDelta = new Vector2(diameter, diameter);
        rt.localPosition = Vector3.zero;
        rt.localRotation = Quaternion.identity;
        rt.localScale = Vector3.one;
    }

    public void SetSprite(Sprite sprite)
    {
        circleSprite = sprite;
    }
}
