using System;

namespace ResortEmpire.NPC
{
    /// <summary>Need for a hotel room, satisfied at the Reception. Active from the first wave.</summary>
    [Serializable]
    public sealed class RoomsNeed : Need
    {
        public override string DisplayName => "Rooms";
    }
}
