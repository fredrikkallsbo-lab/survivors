using UnityEngine;
using UnityEngine.UI;

namespace Units.HealthDisplay
{
    public class PlayerScreenHealthBar : MonoBehaviour, IHealthDisplayer
    {
        [SerializeField] private Vector2 barSize = new Vector2(600f, 30f);
        [SerializeField] private Vector2 barOffset = new Vector2(0f, 25f);
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private int canvasSortingOrder = 100;

        private Image _fillImage;
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
            if (_canvasRoot != null)
            {
                return;
            }

            _canvasRoot = new GameObject("PlayerHealthCanvas");
            _canvasRoot.transform.SetParent(null);

            var canvas = _canvasRoot.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = canvasSortingOrder;

            var scaler = _canvasRoot.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.matchWidthOrHeight = 1f;

            _canvasRoot.AddComponent<GraphicRaycaster>();

            var background = new GameObject("HealthBarBackground");
            background.transform.SetParent(_canvasRoot.transform, false);
            var backgroundRect = background.AddComponent<RectTransform>();
            backgroundRect.anchorMin = new Vector2(0.5f, 0f);
            backgroundRect.anchorMax = new Vector2(0.5f, 0f);
            backgroundRect.pivot = new Vector2(0.5f, 0f);
            backgroundRect.sizeDelta = barSize;
            backgroundRect.anchoredPosition = barOffset;

            var backgroundImage = background.AddComponent<Image>();
            backgroundImage.color = Color.black;

            var fill = new GameObject("HealthBarFill");
            fill.transform.SetParent(background.transform, false);
            var fillRect = fill.AddComponent<RectTransform>();
            fillRect.anchorMin = new Vector2(0f, 0f);
            fillRect.anchorMax = new Vector2(1f, 1f);
            fillRect.offsetMin = new Vector2(4f, 4f);
            fillRect.offsetMax = new Vector2(-4f, -4f);
            _fillImage = fill.AddComponent<Image>();
            _fillImage.color = Color.green;
            _fillImage.type = Image.Type.Filled;
            _fillImage.fillMethod = Image.FillMethod.Horizontal;
            _fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
            _fillImage.fillAmount = 1f;

            var textGo = new GameObject("HealthText");
            textGo.transform.SetParent(background.transform, false);
            var textRect = textGo.AddComponent<RectTransform>();
            textRect.anchorMin = Vector2.zero;
            textRect.anchorMax = Vector2.one;
            textRect.offsetMin = Vector2.zero;
            textRect.offsetMax = Vector2.zero;

            _healthText = textGo.AddComponent<Text>();
            _healthText.alignment = TextAnchor.MiddleCenter;
            _healthText.color = Color.white;
            //_healthText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        private void UpdateVisual(float fillAmount)
        {
            if (_fillImage == null || _healthText == null)
            {
                return;
            }

            _fillImage.fillAmount = fillAmount;
            int currentHealth = Mathf.RoundToInt(maxHealth * fillAmount);
            _healthText.text = $"{currentHealth}/{maxHealth}";
        }

        private void OnDestroy()
        {
            if (_canvasRoot != null)
            {
                Destroy(_canvasRoot);
            }
        }
    }
}
