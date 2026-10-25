using UnityEngine;

namespace ResortEmpire.World
{
    /// <summary>
    /// The cruise ship that brings guests to the island. NPCs are spawned at
    /// <see cref="SpawnPoint"/> and walk from there to the buildings.
    /// </summary>
    public sealed class CruiseShip : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;

        /// <summary>Where new NPCs appear. Falls back to the ship itself when not set.</summary>
        public Transform SpawnPoint => spawnPoint != null ? spawnPoint : transform;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(SpawnPoint.position, 0.25f);
        }
    }
}
