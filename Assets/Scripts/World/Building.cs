using System;
using System.Collections.Generic;
using UnityEngine;

namespace ResortEmpire.World
{
    public class Building : MonoBehaviour
    {
        [SerializeField] private string displayName = "Building";
        [SerializeField] private Transform entrance;
        [SerializeField, Min(1)] private int capacity = 1;

        private readonly List<GameObject> _visitors = new();

        public event Action<Building> VisitorsChanged;

        public string DisplayName => displayName;

        public Vector3 EntrancePosition => entrance != null ? entrance.position : transform.position;

        public int Capacity => capacity;

        public int VisitorCount => _visitors.Count;

        public bool IsFull => _visitors.Count >= capacity;

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

        protected virtual void OnVisitorEntered(GameObject visitor) { }

        protected virtual void OnVisitorLeft(GameObject visitor) { }
    }
}
