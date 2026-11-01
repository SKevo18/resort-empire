using System;

namespace ResortEmpire.NPC
{
    /// <summary>Need for a meal, satisfied at the Restaurant. Active from the first wave.</summary>
    [Serializable]
    public sealed class FoodNeed : Need
    {
        public override string DisplayName => "Food";
    }
}
