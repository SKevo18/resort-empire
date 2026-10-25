using System;
using UnityEngine;

namespace ResortEmpire.World
{
    /// <summary>
    /// One of the fixed places on the island where the player can build an object
    /// (pool, bar, ...). Holds at most one building, parented under the spot.
    /// </summary>
    public sealed class BuildingSpot : MonoBehaviour
    {
        [SerializeField, Min(0)] private int index;
        [SerializeField] private Vector2 size = new(1.6f, 1.6f);

        private GameObject _building;

        /// <summary>Raised after a building is placed on or removed from this spot.</summary>
        public event Action<BuildingSpot> Changed;

        /// <summary>Position of the spot in the island layout, 0-based.</summary>
        public int Index => index;

        /// <summary>Footprint of the spot in world units.</summary>
        public Vector2 Size => size;

        /// <summary>True when a building stands on this spot.</summary>
        public bool IsOccupied => _building != null;

        /// <summary>The building on this spot, or null.</summary>
        public GameObject Building => _building;

        /// <summary>Instantiates <paramref name="prefab"/> on the spot. Returns false if the spot is taken.</summary>
        public bool TryPlace(GameObject prefab, out GameObject building)
        {
            building = null;
            if (prefab == null || IsOccupied) return false;

            building = Instantiate(prefab, transform.position, Quaternion.identity, transform);
            _building = building;
            Changed?.Invoke(this);
            return true;
        }

        /// <summary>Destroys the building on this spot, if any.</summary>
        public void Clear()
        {
            if (!IsOccupied) return;

            Destroy(_building);
            _building = null;
            Changed?.Invoke(this);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = IsOccupied ? new Color(1f, 0.6f, 0.2f, 0.8f) : new Color(0.3f, 1f, 0.5f, 0.8f);
            Gizmos.DrawWireCube(transform.position, size);
        }
    }
}
