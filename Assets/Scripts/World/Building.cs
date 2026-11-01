using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResortEmpire.World
{
    public class Building : MonoBehaviour
    {
        public const int MaxLevel = 3;

        [SerializeField] private string displayName = "Building";
        [SerializeField] private Transform entrance;
        [SerializeField, Min(1)] private int capacity = 1;
        [SerializeField, Range(1, MaxLevel)] private int level = 1;

        // Capacity for levels 1..MaxLevel; when empty, capacity is used for every level.
        [SerializeField] private int[] capacityPerLevel = Array.Empty<int>();

        private readonly List<GameObject> _visitors = new();

        public event Action<Building> VisitorsChanged;

        public event Action<Building> LevelChanged;

        public string DisplayName => displayName;

        public Vector3 EntrancePosition => entrance != null ? entrance.position : transform.position;

        public int Level => level;

        public bool CanUpgrade => level < MaxLevel;

        public int Capacity => level <= capacityPerLevel.Length ? Mathf.Max(1, capacityPerLevel[level - 1]) : capacity;

        public int VisitorCount => _visitors.Count;

        public bool IsFull => _visitors.Count >= Capacity;

        public bool HasVisitor(GameObject visitor) => _visitors.Contains(visitor);

        public bool TryEnter(GameObject visitor)
        {
            if (visitor == null || IsFull || _visitors.Contains(visitor)) return false;

            _visitors.Add(visitor);
            OnVisitorEntered(visitor);
            VisitorsChanged?.Invoke(this);
            return true;
        }

        public void Leave(GameObject visitor)
        {
            if (!_visitors.Remove(visitor)) return;

            OnVisitorLeft(visitor);
            VisitorsChanged?.Invoke(this);
        }

        // Raises the level by one; paying for the upgrade is up to the caller.
        public bool TryUpgrade()
        {
            if (!CanUpgrade) return false;

            level++;
            LevelChanged?.Invoke(this);
            VisitorsChanged?.Invoke(this);
            return true;
        }

        protected virtual void OnVisitorEntered(GameObject visitor) { }

        protected virtual void OnVisitorLeft(GameObject visitor) { }
    }
}
