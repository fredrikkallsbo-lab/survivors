using UnityEngine;

namespace Units.Anvil.AnvilAbilities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FieryRuneVisual : MonoBehaviour
    {
        
        public static FieryRuneVisual CreateFromResources(GameObject unitGameObject, string resourcePath, int sortingOrderOffset, float scale)
        {
            // resourcePath must be like "Sprites/Random/FieryRune"
            var sprite = UnityEngine.Resources.Load<Sprite>(resourcePath);
            var comp = unitGameObject.GetComponent<FieryRuneVisual>() ?? unitGameObject.AddComponent<FieryRuneVisual>();

            var runeGO = new GameObject("FieryRune");
            runeGO.transform.SetParent(unitGameObject.transform, false);
            runeGO.transform.localPosition = Vector3.zero;
            runeGO.transform.localScale = Vector3.one * Mathf.Max(0.0001f, scale);

            var sr = runeGO.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;

            // (optional but helpful) put it above the unit's sprite if one exists
            var parentSR = unitGameObject.GetComponent<SpriteRenderer>();
            if (parentSR != null)
            {
                sr.sortingLayerID = parentSR.sortingLayerID;
                sr.sortingOrder   = parentSR.sortingOrder + sortingOrderOffset;
            }

            return comp;
        }
    }
}