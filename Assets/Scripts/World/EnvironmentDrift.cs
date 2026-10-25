using UnityEngine;

namespace ResortEmpire.World
{
    /// <summary>
    /// Moves an object slowly along X and wraps it to the other side once it leaves
    /// <see cref="range"/> (in local units around its start position), e.g. a cloud.
    /// </summary>
    public sealed class EnvironmentDrift : MonoBehaviour
    {
        [SerializeField] private float speed = 0.3f;
        [SerializeField, Min(0.1f)] private float range = 12f;
        [SerializeField, Range(0f, 1f)] private float bobAmplitude = 0.05f;

        private Vector3 _start;
        private float _offset;

        private void Awake()
        {
            _start = transform.localPosition;
        }

        private void Update()
        {
            float half = range * 0.5f;
            _offset = Mathf.Repeat(_offset + speed * Time.deltaTime + half, range) - half;
            float bob = Mathf.Sin(Time.time * 0.7f) * bobAmplitude;
            transform.localPosition = _start + new Vector3(_offset, bob, 0f);
        }
    }
}
