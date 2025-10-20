using UnityEngine;
using UnityEngine.UI;

namespace Units.HealthDisplay
{
    public class PlayerScreenHealthText : MonoBehaviour, IHealthDisplayer
    {
        [Header("Settings")]
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int canvasSortingOrder = 100;
        [SerializeField] private Font uiFont;

        private Text _healthText;
        private GameObject _canvasRoot;
        private float _currentFill = 1f;

        private void Awake()
        {
            CreateUi();
            UpdateVisual(_currentFill);
        }

        public void SetFill(float percent)
        {
            _currentFill = Mathf.Clamp01(percent);
            UpdateVisual(_currentFill);
        }

        public void SetMaxHealth(int health)
        {
            maxHealth = Mathf.Max(1, health);
            UpdateVisual(_currentFill);
        }

        private void CreateUi()
        {
            if (_canvasRoot != null) return;

            // Create Canvas
            _canvasRoot = new GameObject("PlayerHealthCanvas");
            _canvasRoot.transform.SetParent(null);

            var canvas = _canvasRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = canvasSortingOrder;

            var scaler = _canvasRoot.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            _canvasRoot.AddComponent<GraphicRaycaster>();

            // Create text
            var textGo = new GameObject("HealthText");
            textGo.transform.SetParent(_canvasRoot.transform, false);

            var textRect = textGo.AddComponent<RectTransform>();
            textRect.anchorMin = new Vector2(0f, 0f);
            textRect.anchorMax = new Vector2(0f, 0f);
            textRect.pivot = new Vector2(0f, 0f);
            textRect.anchoredPosition = new Vector2(20f, 20f); // bottom-left corner padding

            _healthText = textGo.AddComponent<Text>();
            _healthText.alignment = TextAnchor.LowerLeft;
            _healthText.color = Color.white;
            _healthText.font = uiFont != null 
                ? uiFont 
                : Font.CreateDynamicFontFromOSFont("Arial", 24);
            _healthText.fontSize = 24;
        }

        private void UpdateVisual(float fillAmount)
        {
            if (_healthText == null) return;

            int currentHealth = Mathf.RoundToInt(maxHealth * fillAmount);
            float percent = fillAmount * 100f;
            _healthText.text = $"HP: {percent:0}% ({currentHealth}/{maxHealth})";
        }

        private void OnDestroy()
        {
            if (_canvasRoot != null)
                Destroy(_canvasRoot);
        }
    }
}