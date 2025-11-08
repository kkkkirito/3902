//Dillon Brigode Au25 part of GameplayState refactor
using System;
using System.Collections.Generic;
using Sprint_0.Rooms;

namespace Sprint_0.States.Gameplay
{
    public sealed class RoomNavigator
    {
        private readonly List<Room> _rooms;
        public int Index { get; private set; }
        public Room Current => _rooms.Count == 0 ? null : _rooms[Index];

        public event Action<Room> RoomChanged;

        public RoomNavigator(List<Room> rooms, int startIndex = 0)
        {
            _rooms = rooms ?? new List<Room>();
            Index = Math.Clamp(startIndex, 0, Math.Max(0, _rooms.Count - 1));
        }

        public void SwitchTo(int i)
        {
            if (i < 0 || i >= _rooms.Count || i == Index) return;
            Index = i;
            RoomChanged?.Invoke(Current);
        }

        public void Next()
        {
            if (_rooms.Count == 0) return;
            Index = (Index + 1) % _rooms.Count;
            RoomChanged?.Invoke(Current);
        }

        public void Previous()
        {
            if (_rooms.Count == 0) return;
            Index = (Index - 1 + _rooms.Count) % _rooms.Count;
            RoomChanged?.Invoke(Current);
        }
    }
}
