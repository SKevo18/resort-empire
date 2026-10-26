using System.Collections;
using ResortEmpire.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace ResortEmpire.UI
{
    [RequireComponent(typeof(Toggle))]
    public sealed class UIToggle : MonoBehaviour
    {
        [SerializeField] private RectTransform knob;
        [SerializeField] private Graphic track;
        [SerializeField] private Color offColor = new(0.35f, 0.4f, 0.45f, 1f);
        [SerializeField] private Color onColor = new(0.17f, 0.55f, 0.62f, 1f);
        [SerializeField, Min(0f)] private float knobTravel = 28f;
        [SerializeField, Min(0.01f)] private float slideSeconds = 0.12f;
        [SerializeField] private AudioClip clickSound;
        [SerializeField, Range(0f, 1f)] private float clickVolume = 1f;

        private Toggle _toggle;
        private float _progress;
        private Coroutine _routine;

        public Toggle Toggle => _toggle;

        private void Awake()
        {
            _toggle = GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(HandleValueChanged);
            SnapToValue();
        }

        private void OnDestroy()
        {
            _toggle.onValueChanged.RemoveListener(HandleValueChanged);
        }

        private void OnEnable()
        {
            if (_toggle != null) SnapToValue();
        }

        private void OnDisable()
        {
            _routine = null;
        }

        private void HandleValueChanged(bool isOn)
        {
            UiSound.Play(clickSound, clickVolume);
            if (!isActiveAndEnabled)
            {
                SnapToValue();
                return;
            }

            if (_routine != null) StopCoroutine(_routine);
            _routine = StartCoroutine(SlideTo(isOn ? 1f : 0f));
        }

        private IEnumerator SlideTo(float target)
        {
            while (!Mathf.Approximately(_progress, target))
            {
                _progress = Mathf.MoveTowards(_progress, target, Time.unscaledDeltaTime / slideSeconds);
                ApplyProgress();
                yield return null;
            }

            _routine = null;
        }

        private void SnapToValue()
        {
            _progress = _toggle.isOn ? 1f : 0f;
            ApplyProgress();
        }

        private void ApplyProgress()
        {
            Vector2 position = knob.anchoredPosition;
            position.x = Mathf.Lerp(-knobTravel, knobTravel, _progress);
            knob.anchoredPosition = position;
            track.color = Color.Lerp(offColor, onColor, _progress);
        }
    }
}
