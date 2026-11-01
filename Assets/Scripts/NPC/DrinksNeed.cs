using System;

namespace ResortEmpire.NPC
{
    /// <summary>Need for a drink, satisfied at the Beach Bar. Active from the first wave.</summary>
    [Serializable]
    public sealed class DrinksNeed : Need
    {
        public override string DisplayName => "Drinks";
    }
}
