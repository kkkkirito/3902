using Microsoft.Xna.Framework;
using Sprint_0.Rooms;
using System.Collections.Generic;
using System.Diagnostics;

namespace Sprint_0.Systems
{
    public class RoomTransition
    {
        public int TargetRoomId { get; set; }
        public Rectangle TriggerArea { get; set; }
        public Vector2 SpawnPosition { get; set; }
        public TransitionDirection Direction { get; set; }
    }

    public enum TransitionDirection
    {
        Left,
        Right,
        Up,
        Down,
        Door
    }

    public class RoomTransitionManager
    {
        private readonly Dictionary<int, List<RoomTransition>> _transitions = new();
        private const int EDGE_WIDTH = 32;  // Width of edge trigger zones
        private const int DOOR_WIDTH = 48;  // Width of door areas
        private const int SAFE_DISTANCE = 144; // Safe distance from edge to avoid immediate re-trigger

        public RoomTransitionManager()
        {
            SetupTransitions();
        }

        private void SetupTransitions()
        {
            // Room IDs based on the order in RoomLoader:
            // 0 = Palace Exterior
            // 1 = Dungeon Room 1  
            // 2 = Dungeon Room 2
            // 3 = Dungeon Room 3
            // 4 = Dungeon Room 4
            // 5 = Dungeon Room 5
            // 6 = Dungeon Room 6
            // 7 = Dungeon Room 7

            // Palace Exterior (Room 0) transitions
            AddTransition(0, new RoomTransition
            {
                TargetRoomId = 1,
                TriggerArea = new Rectangle(608, 120, DOOR_WIDTH, 64),
                SpawnPosition = new Vector2(64, 160), 
                Direction = TransitionDirection.Door
            });

            // Dungeon Room 1 (Index 1) transitions
            // Left exit back to Palace Exterior
            AddTransition(1, new RoomTransition
            {
                TargetRoomId = 0,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480), // Left edge
                SpawnPosition = new Vector2(560, 120),
                Direction = TransitionDirection.Left
            });

            // Right exit to Room 2
            AddTransition(1, new RoomTransition
            {
                TargetRoomId = 2,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), // Right edge
                SpawnPosition = new Vector2(128, 160),
                Direction = TransitionDirection.Right
            });

            // Dungeon Room 2 (Index 2) transitions
            // Left exit back to Room 1
            AddTransition(2, new RoomTransition
            {
                TargetRoomId = 1,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480),
                SpawnPosition = new Vector2(1024 - SAFE_DISTANCE, 160),
                Direction = TransitionDirection.Left
            });

            // Right exit to Room 3
            AddTransition(2, new RoomTransition
            {
                TargetRoomId = 3,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480),
                SpawnPosition = new Vector2(64, 160),
                Direction = TransitionDirection.Right
            });

            // Dungeon Room 3 (Index 3) transitions  
            // Left exit back to Room 2
            AddTransition(3, new RoomTransition
            {
                TargetRoomId = 2,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480),
                SpawnPosition = new Vector2(1024 - SAFE_DISTANCE, 160),
                Direction = TransitionDirection.Left
            });

            // Door to Room 4
            AddTransition(3, new RoomTransition
            {
                TargetRoomId = 4,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), 
                SpawnPosition = new Vector2(64, 160), 
                Direction = TransitionDirection.Door
            });

            // Dungeon Room 4 (Index 4) transitions
            // Door back to Room 3  
            AddTransition(4, new RoomTransition
            {
                TargetRoomId = 3,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480),
                SpawnPosition = new Vector2(1024 - SAFE_DISTANCE, 160),
                Direction = TransitionDirection.Down
            });

            // Right exit to Room 5
            AddTransition(4, new RoomTransition
            {
                TargetRoomId = 5,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480),
                SpawnPosition = new Vector2(64, 160),
                Direction = TransitionDirection.Right
            });

            // Dungeon Room 5 (Index 5) transitions
            // Left exit back to Room 4
            AddTransition(5, new RoomTransition
            {
                TargetRoomId = 4,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480),
                SpawnPosition = new Vector2(1024 - SAFE_DISTANCE, 160),
                Direction = TransitionDirection.Left
            });

            // Door to Room 6
            AddTransition(5, new RoomTransition
            {
                TargetRoomId = 6,
                TriggerArea = new Rectangle(624, 120, DOOR_WIDTH, 64),
                SpawnPosition = new Vector2(160, 160), 
                Direction = TransitionDirection.Door
            });

            // Dungeon Room 6 (Index 6) transitions
            // Door back to Room 5
            AddTransition(6, new RoomTransition
            {
                TargetRoomId = 5,
                TriggerArea = new Rectangle(112, 0, EDGE_WIDTH, 480),
                SpawnPosition = new Vector2(1024 - SAFE_DISTANCE, 160),
                Direction = TransitionDirection.Door
            });

            // Right exit to Room 7 - FIXED SPAWN POSITION
            AddTransition(6, new RoomTransition
            {
                TargetRoomId = 7,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480),
                SpawnPosition = new Vector2(64, 160), 
                Direction = TransitionDirection.Right
            });

            // Dungeon Room 7 (Index 7) transitions
            AddTransition(7, new RoomTransition
            {
                TargetRoomId = 6,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480),
                SpawnPosition = new Vector2(1024 - SAFE_DISTANCE, 64), 
                Direction = TransitionDirection.Left
            });
        }

        private void AddTransition(int roomId, RoomTransition transition)
        {
            if (!_transitions.ContainsKey(roomId))
            {
                _transitions[roomId] = new List<RoomTransition>();
            }
            _transitions[roomId].Add(transition);

            Debug.WriteLine($"Added transition: Room {roomId} -> Room {transition.TargetRoomId} at {transition.TriggerArea}");
        }

        public RoomTransition CheckTransition(int currentRoomId, Rectangle playerBounds)
        {
            if (!_transitions.ContainsKey(currentRoomId))
            {
                Debug.WriteLine($"No transitions defined for room {currentRoomId}");
                return null;
            }

            foreach (var transition in _transitions[currentRoomId])
            {
                if (transition.TriggerArea.Intersects(playerBounds))
                {
                    Debug.WriteLine($"Transition triggered: Room {currentRoomId} -> Room {transition.TargetRoomId}");
                    Debug.WriteLine($"  Player will spawn at: {transition.SpawnPosition}");
                    return transition;
                }
            }

            return null;
        }
    }
}