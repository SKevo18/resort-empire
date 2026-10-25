using UnityEngine;

namespace ResortEmpire.World
{
    /// <summary>
    /// Gently rotates an object back and forth around its pivot, e.g. a palm tree in the wind.
    /// Put the pivot at the bottom of the sprite so the top moves and the trunk stays put.
    /// </summary>
    public sealed class EnvironmentSway : MonoBehaviour
    {
        [SerializeField, Range(0f, 20f)] private float angle = 4f;
        [SerializeField, Min(0.01f)] private float period = 3f;
        [Tooltip("Random start offset so several trees do not move in sync.")]
        [SerializeField] private bool randomPhase = true;

        private float _phase;
        private Quaternion _baseRotation;

        private void Awake()
        {
            _baseRotation = transform.localRotation;
            _phase = randomPhase ? Random.value * period : 0f;
        }

        private void Update()
        {
            float t = (Time.time + _phase) / period * Mathf.PI * 2f;
            transform.localRotation = _baseRotation * Quaternion.Euler(0f, 0f, Mathf.Sin(t) * angle);
        }
    }
}
