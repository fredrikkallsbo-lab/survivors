using System;
using System.Collections.Generic;
using Units.GeneralUnit.DeathManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Battlefield.GameMechanics.Combat.BattlefieldController
{
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private BattlefieldController battlefieldController;
        private IEventBus _bus;
        private IDisposable _deathSub;

        [Header("UI")] [SerializeField] private Canvas canvasPrefab; // Optional: assign a prefab
        [SerializeField] private Font uiFont;
        [SerializeField] private int fontSize = 36;

        private bool _shown;
        private Canvas _canvas;

        private void OnEnable()
        {
            _bus = battlefieldController.GetEventBus();
            if (_bus == null)
            {
                Debug.LogError("GameOverController: busProvider must implement IEventBus.");
                return;
            }

            _deathSub = _bus.Subscribe<PlayerDeathEvent>(HandlePlayerDeath);
        }

        private void OnDisable()
        {
            _deathSub?.Dispose();
            _deathSub = null;
        }

        private void HandlePlayerDeath(PlayerDeathEvent e)
        {
            if (_shown) return;
            _shown = true;

            Time.timeScale = 0f; // Pause game
            BuildUi(e?.Reason ?? "You died");
        }

        private void BuildUi(string reason)
        {
            if (canvasPrefab != null)
            {
                _canvas = Instantiate(canvasPrefab);
            }
            else
            {
                var root = new GameObject("GameOverCanvas");
                _canvas = root.AddComponent<Canvas>();
                _canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                _canvas.sortingOrder = 1000;

                var scaler = root.AddComponent<CanvasScaler>();
                scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
                scaler.referenceResolution = new Vector2(1920, 1080);

                root.AddComponent<GraphicRaycaster>();
            }

            // Fullscreen dim panel
            var panel = new GameObject("Panel");
            panel.transform.SetParent(_canvas.transform, false);
            var pr = panel.AddComponent<RectTransform>();
            pr.anchorMin = Vector2.zero;
            pr.anchorMax = Vector2.one;
            pr.offsetMin = Vector2.zero;
            pr.offsetMax = Vector2.zero;

            var img = panel.AddComponent<Image>();
            img.color = new Color(0f, 0f, 0f, 0.65f);

            // Title + reason
            MakeText(panel.transform, "DEFEAT", new Vector2(0.5f, 0.7f), TextAnchor.MiddleCenter, fontSize + 12);
            MakeText(panel.transform, reason, new Vector2(0.5f, 0.6f), TextAnchor.MiddleCenter, fontSize);

            // Buttons
            MakeButton(panel.transform, "Restart", new Vector2(0.5f, 0.45f), OnRestart);
            MakeButton(panel.transform, "Quit", new Vector2(0.5f, 0.35f), OnQuit);
        }

        private Text MakeText(Transform parent, string content, Vector2 anchor, TextAnchor align, int size)
        {
            var go = new GameObject("Text");
            go.transform.SetParent(parent, false);

            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = anchor;
            rt.anchorMax = anchor;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(900, 80);

            var text = go.AddComponent<Text>();
            text.text = content;
            text.font = uiFont != null ? uiFont : Font.CreateDynamicFontFromOSFont("Arial", size);
            text.fontSize = size;
            text.alignment = align;
            text.color = Color.white;
            return text;
        }

        private Button MakeButton(Transform parent, string label, Vector2 anchor,
            UnityEngine.Events.UnityAction onClick)
        {
            var go = new GameObject(label + "Button");
            go.transform.SetParent(parent, false);

            var rt = go.AddComponent<RectTransform>();
            rt.anchorMin = anchor;
            rt.anchorMax = anchor;
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.anchoredPosition = Vector2.zero;
            rt.sizeDelta = new Vector2(260, 56);

            var img = go.AddComponent<Image>();
            img.color = new Color(1f, 1f, 1f, 0.12f);

            var btn = go.AddComponent<Button>();
            btn.onClick.AddListener(onClick);

            var txt = MakeText(go.transform, label, new Vector2(0.5f, 0.5f), TextAnchor.MiddleCenter, 28);
            txt.color = Color.white;

            return btn;
        }

        private void OnRestart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        private void OnQuit()
        {
            Time.timeScale = 1f;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
}