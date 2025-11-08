using UnityEngine;

namespace Units.Anvil.AnvilAbilities.Chains
{
    [RequireComponent(typeof(LineRenderer))]
    public class ChainAnchor : MonoBehaviour
    {
        private ChainAnchor _nextAnchor;
        private ChainAnchor _previousAnchor;

        private LineRenderer _lineRenderer;

        private static readonly Color StartColor = new Color(1f, 0.55f, 0f); // dark orange-ish start
        private static readonly Color EndColor   = new Color(0.7f, 0.3f, 0f); // darker end

        private bool _isPulled;
        private float _pullSpeed = 0.5f;

        public void Init(ChainAnchor previousAnchor, ChainAnchor nextAnchor)
        {
            SetPreviousAnchor(previousAnchor);
            SetNextAnchor(nextAnchor);
            StartPulling(_pullSpeed);
        }

        private void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();

            // Ensure world-space drawing
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.positionCount = 2;

            // Line width
            _lineRenderer.startWidth = 0.05f;
            _lineRenderer.endWidth   = 0.05f;

            // Material & color
            _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            _lineRenderer.startColor = StartColor;
            _lineRenderer.endColor   = EndColor;

            // Hide line initially if there’s no next anchor
            _lineRenderer.enabled = false;
        }

        public void SetNextAnchor(ChainAnchor next)
        {
            _nextAnchor = next;
            _lineRenderer.enabled = _nextAnchor != null;
        }

        public void SetPreviousAnchor(ChainAnchor prev)
        {
            _previousAnchor = prev;
        }

        public void StartPulling(float speed)
        {
            _isPulled = true;
            _pullSpeed = speed;
        }

        private void Update()
        {
            // 1️⃣ Update line renderer (draw line to the NEXT anchor only)
            if (_nextAnchor != null)
            {
                _lineRenderer.enabled = true;
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, _nextAnchor.transform.position);
            }
            else
            {
                _lineRenderer.enabled = false;
            }

            // 2️⃣ Pulling motion toward BOTH previous and next anchors (if any)
            if (_isPulled)
            {
                Vector3 totalForce = Vector3.zero;
                int count = 0;

                if (_previousAnchor != null)
                {
                    totalForce += (_previousAnchor.transform.position - transform.position);
                    count++;
                }

                if (_nextAnchor != null)
                {
                    totalForce += (_nextAnchor.transform.position - transform.position);
                    count++;
                }

                if (count > 0)
                {
                    Vector3 direction = totalForce.normalized;
                    transform.position += direction * (_pullSpeed * Time.deltaTime);
                }
            }
        }
    }
}
