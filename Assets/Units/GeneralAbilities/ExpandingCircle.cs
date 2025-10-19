using UnityEngine;

namespace Units.Abilities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class ExpandingCircle : MonoBehaviour
    {
        [Header("Animation")] [SerializeField] private float duration = 0.6f;
        [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private AnimationCurve alphaCurve = AnimationCurve.EaseInOut(0, 1, 1, 0);

        [Header("Visuals")] [SerializeField] private Color tintColor = Color.white;
        [SerializeField] private Color innerGlowColor = new Color(1f, 0.86f, 0.35f, 1f);
        [SerializeField] private Color outerGlowColor = new Color(0.66f, 0.07f, 0.01f, 1f);
        [SerializeField, Range(0.05f, 0.9f)] private float ringThickness = 0.45f;
        [SerializeField, Range(0.01f, 0.4f)] private float edgeFeather = 0.18f;
        [SerializeField] private Sprite circleSprite; // can be generated at runtime
        [SerializeField] private int orderInLayer = 10;

        private SpriteRenderer sr;
        private float t;
        private float targetDiameter;
        private Color currentTint;

        void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            if (sr == null)
                sr = GetComponentInChildren<SpriteRenderer>();

            // Prefer an already-assigned sprite; otherwise generate one
            if (circleSprite == null)
                circleSprite = sr.sprite != null ? sr.sprite : GenerateCircleSprite(128);

            sr.sprite = circleSprite;

            // Fix pink (missing shader)
            sr.sharedMaterial = null; // falls back to default Sprite shader
            sr.sortingOrder = orderInLayer;
            currentTint = tintColor;
            sr.color = currentTint;
        }

        /// <summary>
        /// Play an expanding circle centered at pos, ending at 'radius' in world units.
        /// </summary>
        public void Play(Vector3 pos, float radius, Color? overrideColor = null, float? overrideDuration = null)
        {
            transform.position = pos;
            targetDiameter = radius * 2f;
            t = 0f;
            currentTint = overrideColor ?? tintColor;
            if (overrideDuration.HasValue) duration = overrideDuration.Value;

            // start at zero scale (in world units)
            SetScaleForDiameter(0.0001f); // tiny, not zero to avoid NaNs
            SetColorAlpha(1f);

            enabled = true;
            gameObject.SetActive(true);
        }

        void Update()
        {
            if (duration <= 0f)
            {
                SetScaleForDiameter(targetDiameter);
                enabled = false;
                return;
            }

            t += Time.deltaTime / duration;
            float k = Mathf.Clamp01(t);

            // scale
            float s = scaleCurve.Evaluate(k);
            SetScaleForDiameter(Mathf.Lerp(0f, targetDiameter, s));

            // fade
            float a = alphaCurve.Evaluate(k);
            SetColorAlpha(a);

            if (k >= 1f)
            {
                enabled = false;
                gameObject.SetActive(false); // ready for pooling reuse
            }
        }

        private void SetScaleForDiameter(float worldDiameter)
        {
            // sprite.bounds.size.x is world units at scale=1
            float spriteDiameter = sr.sprite.bounds.size.x;
            float scale = spriteDiameter > 0f ? (worldDiameter / spriteDiameter) : 1f;
            transform.localScale = Vector3.one * scale;
        }

        private void SetColorAlpha(float a)
        {
            var c = currentTint;
            c.a = a;
            sr.color = c;
        }

        // Simple runtime circle sprite
        private Sprite GenerateCircleSprite(int size)
        {
            var tex = new Texture2D(size, size, TextureFormat.ARGB32, false);
            var pixels = new Color32[size * size];
            Vector2 center = new(size * 0.5f, size * 0.5f);
            float radius = size * 0.5f;

            float innerEdge = Mathf.Clamp01(1f - ringThickness);
            float innerFadeStart = Mathf.Clamp01(innerEdge - edgeFeather);
            float outerFadeStart = Mathf.Clamp01(1f - edgeFeather);

            for (int y = 0; y < size; y++)
            {
                int row = y * size;
                float dy = (y + 0.5f) - center.y;
                for (int x = 0; x < size; x++)
                {
                    float dx = (x + 0.5f) - center.x;
                    float dist = Mathf.Sqrt(dx * dx + dy * dy);
                    float normalized = Mathf.Clamp01(dist / radius);

                    float inner = Mathf.Clamp01(Mathf.InverseLerp(innerFadeStart, innerEdge, normalized));
                    float outer = Mathf.Clamp01(1f - Mathf.InverseLerp(outerFadeStart, 1f, normalized));
                    float alpha = Mathf.Clamp01(inner * outer);

                    if (normalized >= 1f)
                    {
                        pixels[row + x] = new Color(0f, 0f, 0f, 0f);
                        continue;
                    }

                    float glowMix = Mathf.Pow(normalized, 0.75f);
                    Color pixelColor = Color.Lerp(innerGlowColor, outerGlowColor, glowMix);
                    pixelColor.a *= alpha;
                    pixels[row + x] = pixelColor;
                }
            }

            tex.SetPixels32(pixels);
            tex.Apply();
            tex.filterMode = FilterMode.Bilinear;
            tex.wrapMode = TextureWrapMode.Clamp;

            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), size);
        }
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log($"Hit enemy: {other.name}");
                // Example: other.GetComponent<Enemy>().TakeDamage(10);
            }
        }
    }
}