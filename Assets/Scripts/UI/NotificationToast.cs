using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ResortEmpire.UI
{
    /// <summary>
    /// Toast notification that slides down from the top edge of its canvas, stays for
    /// <see cref="displaySeconds"/>, then slides back up and deactivates itself.
    /// Expects a top-anchored RectTransform with its pivot on the top edge.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class NotificationToast : MonoBehaviour
    {
        [SerializeField] private TMP_Text titleLabel;
        [SerializeField] private TMP_Text messageLabel;
        [SerializeField] private Button closeButton;
        [SerializeField, Min(0f)] private float displaySeconds = 5f;
        [SerializeField, Min(0.01f)] private float slideSeconds = 0.25f;

        private RectTransform _rect;
        private CanvasGroup _group;
        private Vector2 _shownPosition;
        private float _visibility; // 0 = hidden above the top edge, 1 = fully shown.
        private Coroutine _routine;

        private void Awake()
        {
            _rect = (RectTransform)transform;
            _group = GetComponent<CanvasGroup>();
            _shownPosition = _rect.anchoredPosition;
            closeButton.onClick.AddListener(Dismiss);
            ApplyVisibility();
        }

        private void OnDisable()
        {
            _routine = null;
            _visibility = 0f;
            ApplyVisibility();
        }

        /// <summary>Shows the toast on top of its siblings; restarts the timer if already visible.</summary>
        public void Show(string title, string message)
        {
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
            titleLabel.text = title;
            messageLabel.text = message;
            LayoutRebuilder.ForceRebuildLayoutImmediate(_rect);

            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(ShowThenHide());
        }

        /// <summary>Slides the toast out early. No-op when it is not showing.</summary>
        public void Dismiss()
        {
            if (!isActiveAndEnabled) return;

            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(Hide());
        }

        private IEnumerator ShowThenHide()
        {
            yield return AnimateTo(1f);
            yield return new WaitForSecondsRealtime(displaySeconds);
            yield return Hide();
        }

        private IEnumerator Hide()
        {
            yield return AnimateTo(0f);
            gameObject.SetActive(false);
        }

        private IEnumerator AnimateTo(float target)
        {
            while (_visibility != target)
            {
                yield return null; // Step on frame time, not on the delta of the frame that called Show.
                _visibility = Mathf.MoveTowards(_visibility, target, Time.unscaledDeltaTime / slideSeconds);
                ApplyVisibility();
            }
        }

        private void ApplyVisibility()
        {
            float eased = 1f - Mathf.Pow(1f - _visibility, 3f); // Ease-out in, ease-in out.
            float hiddenY = _rect.rect.height; // Pivot on the top edge: bottom edge rests on the canvas top.
            _rect.anchoredPosition = new Vector2(_shownPosition.x, Mathf.LerpUnclamped(hiddenY, _shownPosition.y, eased));
            _group.alpha = eased;
        }
    }
}
