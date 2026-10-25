using System.Collections;
using ResortEmpire.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ResortEmpire.UI
{
    /// <summary>
    /// Shared behaviour for every game button: shrinks slightly while pressed and plays a
    /// click sound on click. Put it next to a <see cref="Button"/> (the Button prefabs in
    /// <c>Prefabs/UI</c> already have it) and hook game logic to <see cref="Button.onClick"/>.
    /// </summary>
    [RequireComponent(typeof(Button))]
    public sealed class UIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [SerializeField] private AudioClip clickSound;
        [SerializeField, Range(0f, 1f)] private float clickVolume = 1f;
        [SerializeField, Range(0.5f, 1f)] private float pressedScale = 0.95f;
        [SerializeField, Min(0.01f)] private float scaleSeconds = 0.06f;

        private Button _button;
        private Vector3 _baseScale;
        private Coroutine _routine;

        /// <summary>The underlying Unity button.</summary>
        public Button Button => _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _baseScale = transform.localScale;
            _button.onClick.AddListener(PlayClickSound);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(PlayClickSound);
        }

        private void OnDisable()
        {
            _routine = null;
            transform.localScale = _baseScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !_button.IsInteractable()) return;
            ScaleTo(pressedScale);
        }

        public void OnPointerUp(PointerEventData eventData) => ScaleTo(1f);

        public void OnPointerExit(PointerEventData eventData) => ScaleTo(1f);

        private void PlayClickSound() => UiSound.Play(clickSound, clickVolume);

        private void ScaleTo(float factor)
        {
            if (!isActiveAndEnabled) return;
            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(AnimateScale(_baseScale * factor));
        }

        // Unscaled time so buttons still react while the game is paused.
        private IEnumerator AnimateScale(Vector3 target)
        {
            Vector3 start = transform.localScale;
            for (float t = 0f; t < scaleSeconds; t += Time.unscaledDeltaTime)
            {
                transform.localScale = Vector3.Lerp(start, target, t / scaleSeconds);
                yield return null;
            }

            transform.localScale = target;
            _routine = null;
        }
    }
}
