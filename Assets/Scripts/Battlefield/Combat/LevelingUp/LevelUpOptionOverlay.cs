using System;
using System.Collections.Generic;
using Battlefield.Combat.BattlefieldController;
using Battlefield.GameEvents;
using Units.Abilities.AbilityManagement.AbilityUpgrades;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Units.GeneralUnit.Minion
{
    public class LevelUpOptionOverlay : MonoBehaviour
    {
        [Header("Card Settings")] [SerializeField]
        private Vector2 cardSize = new Vector2(240, 320);

        [SerializeField] private float cardSpacing = 24f;
        [SerializeField] private Color cardColor = new Color(0.15f, 0.15f, 0.18f, 0.95f);
        [SerializeField] private Color cardHoverColor = new Color(0.25f, 0.25f, 0.35f, 0.95f);
        [SerializeField] private Color textColor = Color.white;
        [SerializeField] private int fontSize = 22;
        [SerializeField] private bool closeOnClick = true;
        [SerializeField] private Font textFont;
        [SerializeField] private BattlefieldController battlefieldController;
        
        private GameObject overlayRoot;
        private float previousTimeScale = 1f;
        private bool isShowing = false;
        private IEventBus eventBus;

        public void Awake()
        {
            eventBus = battlefieldController.GetEventBus();
            eventBus.Subscribe<PlayerLevelUpEvent>(_ => ShowOptions(4));
        }

        /// <summary>
        /// Call this to open the overlay, pause the game, and show N clickable rectangles.
        /// </summary>
        public void ShowOptions(int count)
        {
            if (isShowing) return;
            isShowing = true;

            EnsureEventSystem();
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;

            // Root canvas
            var canvasGO = new GameObject("LevelUpOverlay_Canvas", typeof(Canvas), typeof(CanvasScaler),
                typeof(GraphicRaycaster));
            overlayRoot = canvasGO;
            var canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            var scaler = canvasGO.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            // Dim background
            var dimGO = new GameObject("Dim", typeof(Image));
            dimGO.transform.SetParent(canvasGO.transform, false);
            var dimImg = dimGO.GetComponent<Image>();
            dimImg.color = new Color(0f, 0f, 0f, 0.55f);
            StretchToParent(dimGO);

            // Cards container
            var container = new GameObject("CardsContainer", typeof(RectTransform), typeof(HorizontalLayoutGroup),
                typeof(ContentSizeFitter));
            container.transform.SetParent(canvasGO.transform, false);
            var rt = container.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;

            var h = container.GetComponent<HorizontalLayoutGroup>();
            h.childAlignment = TextAnchor.MiddleCenter;
            h.spacing = cardSpacing;
            h.childControlWidth = false;
            h.childControlHeight = false;

            var fitter = container.GetComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            List<IAbilityUpgrade> upgrades = battlefieldController.GetWanderer().GetAbilityLevelUpOptions(count);
            // Spawn cards
            for (int i = 0; i < count; i++)
            {
                
                GameObject btnGO = MakeCard(upgrades[i].GetUpgradeString());
                btnGO.transform.SetParent(container.transform, false);

                // Set click behavior
                var button = btnGO.GetComponent<Button>();
                int capturedIndex = i; // capture for closure
                button.onClick.AddListener(() =>
                {
                    Debug.Log($"[LevelUpOptionOverlay] Clicked card #{capturedIndex + 1}");
                    battlefieldController.GetWanderer().AddUpgrade(upgrades[capturedIndex]);
                    if (closeOnClick) Hide();
                });
            }
        }

        /// <summary>
        /// Closes the overlay and resumes the previous time scale.
        /// </summary>
        public void Hide()
        {
            if (!isShowing) return;
            isShowing = false;

            if (overlayRoot != null)
                Destroy(overlayRoot);

            Time.timeScale = previousTimeScale;
        }

        // ---------- helpers ----------

        private void EnsureEventSystem()
        {
            if (FindObjectOfType<EventSystem>() == null)
            {
                var es = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
                DontDestroyOnLoad(es); // keep it around if you want
            }
        }

        private static void StretchToParent(GameObject go)
        {
            var rt = go.GetComponent<RectTransform>();
            if (rt == null) rt = go.AddComponent<RectTransform>();
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
        }

        private GameObject MakeCard(string label)
        {
            var go = new GameObject(label, typeof(RectTransform), typeof(Image), typeof(Button));
            var rt = go.GetComponent<RectTransform>();
            rt.sizeDelta = cardSize;

            var img = go.GetComponent<Image>();
            img.color = cardColor;

            var button = go.GetComponent<Button>();
            var colors = button.colors;
            colors.highlightedColor = cardHoverColor;
            colors.pressedColor = cardHoverColor * 0.9f;
            colors.selectedColor = cardHoverColor;
            button.colors = colors;

            // Create label
            var textGO = new GameObject("Label", typeof(RectTransform), typeof(Text));
            textGO.transform.SetParent(go.transform, false);
            var textRT = textGO.GetComponent<RectTransform>();
            textRT.anchorMin = Vector2.zero;
            textRT.anchorMax = Vector2.one;
            textRT.offsetMin = new Vector2(12, 12);
            textRT.offsetMax = new Vector2(-12, -12);

            var text = textGO.GetComponent<Text>();
            text.text = label; // ✅ this is the visible text
            text.alignment = TextAnchor.MiddleCenter;
            text.color = textColor;
            text.fontSize = fontSize;
            text.font = textFont;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            text.verticalOverflow = VerticalWrapMode.Truncate;

            var outline = textGO.AddComponent<Outline>();
            outline.effectDistance = new Vector2(1f, -1f);
            outline.effectColor = new Color(0f, 0f, 0f, 0.6f);

            var shadow = go.AddComponent<Shadow>();
            shadow.effectDistance = new Vector2(0f, -2f);
            shadow.effectColor = new Color(0f, 0f, 0f, 0.4f);

            return go;
        }

        private void OnDestroy()
        {
            // Safety: if this component is destroyed while paused, resume time.
            if (isShowing)
            {
                Time.timeScale = previousTimeScale;
            }
        }
    }
}