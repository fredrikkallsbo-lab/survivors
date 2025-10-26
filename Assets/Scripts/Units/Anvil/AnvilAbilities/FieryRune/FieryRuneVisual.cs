using UnityEngine;

namespace Units.Anvil.AnvilAbilities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FieryRuneVisual : MonoBehaviour
    {
        private GameObject _runeGO;
        private SpriteRenderer _runeRenderer;

        // --- PUBLIC FACTORY METHODS ---

        /// <summary>
        /// Create the rune visual from a Sprite you've already loaded (e.g., Addressables).
        /// </summary>
        public static FieryRuneVisual Create(GameObject host, Sprite sprite, int sortingOrderOffset = 5, float scale = 1f)
        {
            if (host == null || sprite == null) { Debug.LogError("[FieryRuneVisual] Host or Sprite null."); return null; }

            var comp = host.GetComponent<FieryRuneVisual>() ?? host.AddComponent<FieryRuneVisual>();
            comp.Build(sprite, sortingOrderOffset, scale);
            return comp;
        }

        /// <summary>
        /// Create the rune visual by loading a sprite from Resources.
        /// Place your PNG at Assets/Resources/Art/Units/Sprites/FieryRune.png
        /// and call with resourcePath = "Art/Units/Sprites/FieryRune".
        /// </summary>
        public static FieryRuneVisual CreateFromResources(GameObject host, string resourcePath, int sortingOrderOffset = 5, float scale = 1f)
        {
            if (host == null) { Debug.LogError("[FieryRuneVisual] Host null."); return null; }
            var sprite = UnityEngine.Resources.Load<Sprite>(resourcePath);
            if (sprite == null)
            {
                Debug.LogError($"[FieryRuneVisual] Resources.Load failed at '{resourcePath}'. " +
                               "Ensure the sprite is under Assets/Resources/ and path has no extension.");
                return null;
            }
            return Create(host, sprite, sortingOrderOffset, scale);
        }

        // --- RUNTIME API ---

        /// <summary> Remove/destroy the rune visual (keeps this component). </summary>
        public void Clear()
        {
            if (_runeGO != null) Destroy(_runeGO);
            _runeGO = null;
            _runeRenderer = null;
        }

        /// <summary> Remove both the rune visual and this component. </summary>
        public void DisposeAndRemove() { Clear(); Destroy(this); }

        // --- INTERNAL BUILD ---

        private void Build(Sprite sprite, int sortingOrderOffset, float scale)
        {
            Clear();

            // Create child object
            _runeGO = new GameObject("FieryRune");
            _runeGO.transform.SetParent(transform, false);
            _runeGO.transform.localPosition = Vector3.zero;
            _runeGO.transform.localScale = Vector3.one * Mathf.Max(0.0001f, scale);

            _runeRenderer = _runeGO.AddComponent<SpriteRenderer>();
            _runeRenderer.sprite = sprite;

            // Match parent's sorting layer, then sit above it
            var parentSR = GetComponent<SpriteRenderer>();
            if (parentSR != null)
            {
                _runeRenderer.sortingLayerID = parentSR.sortingLayerID;
                _runeRenderer.sortingOrder   = parentSR.sortingOrder + sortingOrderOffset;
            }
            else
            {
                // If the host lacks a SpriteRenderer, still render visibly
                _runeRenderer.sortingOrder = 100;
            }
        }
    }
}