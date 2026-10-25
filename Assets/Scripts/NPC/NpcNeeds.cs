using System.Collections.Generic;
using UnityEngine;

namespace ResortEmpire.NPC
{
    /// <summary>
    /// Holds the needs of one NPC and decays them every frame. Concrete <see cref="Need"/>
    /// subclasses are added in the Inspector (serialized by reference) or from code.
    /// </summary>
    public sealed class NpcNeeds : MonoBehaviour
    {
        [SerializeReference] private List<Need> needs = new();

        /// <summary>All needs of this NPC.</summary>
        public IReadOnlyList<Need> Needs => needs;

        private void Update()
        {
            float deltaTime = Time.deltaTime;
            foreach (var need in needs)
            {
                need?.Tick(deltaTime);
            }
        }

        /// <summary>Adds a need; ignores null and duplicates of the same type.</summary>
        public void Add(Need need)
        {
            if (need == null || TryGet(need.GetType(), out _)) return;
            needs.Add(need);
        }

        /// <summary>Returns the need of type <typeparamref name="T"/>, or null if the NPC does not have it.</summary>
        public T Get<T>() where T : Need
        {
            return TryGet(typeof(T), out var need) ? (T)need : null;
        }

        /// <summary>The least satisfied need, i.e. what the NPC wants most. Null when there are no needs.</summary>
        public Need MostUrgent()
        {
            Need urgent = null;
            foreach (var need in needs)
            {
                if (need != null && (urgent == null || need.Satisfaction < urgent.Satisfaction))
                {
                    urgent = need;
                }
            }

            return urgent;
        }

        private bool TryGet(System.Type type, out Need found)
        {
            foreach (var need in needs)
            {
                if (need != null && need.GetType() == type)
                {
                    found = need;
                    return true;
                }
            }

            found = null;
            return false;
        }
    }
}
