using System;
using ResortEmpire.NPC;
using UnityEngine;

namespace ResortEmpire.World
{
    /// <summary>
    /// Building that satisfies one guest need of type <typeparamref name="TNeed"/>
    /// (Reception → Rooms, Beach Bar → Drinks, ...).
    /// </summary>
    public abstract class NeedBuilding<TNeed> : Building, INeedBuilding where TNeed : Need
    {
        [SerializeField, Range(0f, 1f)] private float satisfyAmount = 1f;

        /// <summary>The need type this building serves.</summary>
        public Type ServedNeed => typeof(TNeed);

        /// <summary>How much satisfaction one visit restores.</summary>
        public float SatisfyAmount => satisfyAmount;

        /// <summary>
        /// Finishes the visit of <paramref name="visitor"/>: satisfies its need and lets it
        /// leave. Returns false if the visitor is not inside.
        /// </summary>
        public bool Serve(GameObject visitor)
        {
            if (!HasVisitor(visitor)) return false;

            if (visitor.TryGetComponent(out NpcNeeds needs))
            {
                needs.Get<TNeed>()?.Satisfy(satisfyAmount);
            }

            Leave(visitor);
            return true;
        }
    }

    /// <summary>Non-generic view of <see cref="NeedBuilding{TNeed}"/> for lookups by need type.</summary>
    public interface INeedBuilding
    {
        Type ServedNeed { get; }

        bool Serve(GameObject visitor);
    }
}
