using System;
using UnityEngine;

namespace ResortEmpire.NPC
{
    /// <summary>
    /// Base class for a single NPC need (rooms, food, drinks, ...). Satisfaction is kept in
    /// the 0–1 range: it drops by <see cref="decayPerSecond"/> over time and is restored by
    /// buildings through <see cref="Satisfy"/>. Concrete needs only provide their display name
    /// and may override the tuning defaults.
    /// </summary>
    [Serializable]
    public abstract class Need
    {
        [SerializeField, Range(0f, 1f)] private float satisfaction = 1f;
        [SerializeField, Min(0f)] private float decayPerSecond = 0.01f;
        [SerializeField, Range(0f, 1f)] private float criticalThreshold = 0.25f;

        /// <summary>Raised with the new satisfaction whenever it changes.</summary>
        public event Action<Need, float> Changed;

        /// <summary>Raised once each time satisfaction drops to or below <see cref="CriticalThreshold"/>.</summary>
        public event Action<Need> BecameCritical;

        /// <summary>Name shown in the UI, e.g. "Food".</summary>
        public abstract string DisplayName { get; }

        /// <summary>Current satisfaction, 0 = completely unmet, 1 = fully satisfied.</summary>
        public float Satisfaction => satisfaction;

        /// <summary>How much satisfaction is lost per second.</summary>
        public float DecayPerSecond
        {
            get => decayPerSecond;
            set => decayPerSecond = Mathf.Max(0f, value);
        }

        /// <summary>Satisfaction at or below which the need counts as critical.</summary>
        public float CriticalThreshold
        {
            get => criticalThreshold;
            set => criticalThreshold = Mathf.Clamp01(value);
        }

        /// <summary>True while the need is at or below its critical threshold.</summary>
        public bool IsCritical => satisfaction <= criticalThreshold;

        /// <summary>True when the need is fully satisfied.</summary>
        public bool IsSatisfied => satisfaction >= 1f;

        /// <summary>Applies decay for <paramref name="deltaTime"/> seconds. Call once per frame.</summary>
        public void Tick(float deltaTime)
        {
            if (deltaTime <= 0f || decayPerSecond <= 0f) return;
            SetSatisfaction(satisfaction - decayPerSecond * deltaTime);
        }

        /// <summary>Restores satisfaction by <paramref name="amount"/> (clamped to 1).</summary>
        public void Satisfy(float amount)
        {
            if (amount <= 0f) return;
            SetSatisfaction(satisfaction + amount);
        }

        /// <summary>Sets satisfaction directly, e.g. when spawning an NPC with a random state.</summary>
        public void SetSatisfaction(float value)
        {
            value = Mathf.Clamp01(value);
            if (Mathf.Approximately(value, satisfaction)) return;

            bool wasCritical = IsCritical;
            satisfaction = value;
            OnSatisfactionChanged();
            Changed?.Invoke(this, satisfaction);
            if (!wasCritical && IsCritical) BecameCritical?.Invoke(this);
        }

        /// <summary>Hook for subclasses that react to satisfaction changes.</summary>
        protected virtual void OnSatisfactionChanged() { }

        public override string ToString() => $"{DisplayName}: {satisfaction:P0}";
    }
}
