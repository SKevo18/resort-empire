using System.Collections.Generic;
using UnityEngine;

namespace ResortEmpire.World
{
    /// <summary>
    /// Entry point of the GameWorld scene: knows the cruise ship and the building spots so
    /// other systems (NPC spawning, building, saving) do not have to search the hierarchy.
    /// </summary>
    public sealed class GameWorldController : MonoBehaviour
    {
        [SerializeField] private CruiseShip cruiseShip;
        [SerializeField] private List<BuildingSpot> buildingSpots = new();

        /// <summary>The active controller of the loaded GameWorld scene.</summary>
        public static GameWorldController Instance { get; private set; }

        /// <summary>Ship the guests arrive on.</summary>
        public CruiseShip CruiseShip => cruiseShip;

        /// <summary>All building spots, ordered by <see cref="BuildingSpot.Index"/>.</summary>
        public IReadOnlyList<BuildingSpot> BuildingSpots => buildingSpots;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning($"Second {nameof(GameWorldController)} in the scene, disabling {name}.", this);
                enabled = false;
                return;
            }

            Instance = this;
            buildingSpots.RemoveAll(spot => spot == null);
            buildingSpots.Sort((a, b) => a.Index.CompareTo(b.Index));
        }

        private void OnDestroy()
        {
            if (Instance == this) Instance = null;
        }

        /// <summary>First spot without a building, or null when the island is full.</summary>
        public BuildingSpot FindFreeSpot()
        {
            foreach (var spot in buildingSpots)
            {
                if (!spot.IsOccupied) return spot;
            }

            return null;
        }
    }
}
