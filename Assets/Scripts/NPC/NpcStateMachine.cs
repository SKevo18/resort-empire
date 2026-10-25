using System;
using UnityEngine;

namespace ResortEmpire.NPC
{
    public sealed class NpcStateMachine : MonoBehaviour
    {
        [SerializeField] private NpcState initialState = NpcState.Idle;

        private NpcState _current;
        private float _enteredAt;

        public event Action<NpcState, NpcState> StateChanged;

        public NpcState Current => _current;

        public float SecondsInState => Time.time - _enteredAt;

        private void Awake()
        {
            _current = initialState;
            _enteredAt = Time.time;
        }

        public bool Is(NpcState state) => _current == state;

        public void SetState(NpcState next)
        {
            if (next == _current) return;

            NpcState previous = _current;
            _current = next;
            _enteredAt = Time.time;
            StateChanged?.Invoke(previous, next);
        }
    }
}
