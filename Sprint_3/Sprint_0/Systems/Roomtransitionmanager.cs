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
                SpawnPosition = new Vector2(592, 160), 
                Direction = TransitionDirection.Door
            });

            //Left exit 1 -> 2
            AddTransition(1, new RoomTransition
            {
                TargetRoomId = 2,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480), //Left edge
                SpawnPosition = new Vector2(128, 160),
                Direction = TransitionDirection.Left
            });

            //Right exit 2 -> 1
            AddTransition(2, new RoomTransition
            {
                TargetRoomId = 1,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), // Right edge
                SpawnPosition = new Vector2(32, 160),
                Direction = TransitionDirection.Right    
            });

            //Right exit 1 -> 3
            AddTransition(1, new RoomTransition
            {
                TargetRoomId = 3,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), // Right edge
                SpawnPosition = new Vector2(32, 160),
                Direction = TransitionDirection.Right
            });

            //Left exit 3 -> 1
            AddTransition(3, new RoomTransition
            {
                TargetRoomId = 1,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480), //Left edge
                SpawnPosition = new Vector2(976, 160),
                Direction = TransitionDirection.Left
            });

            //Right exit 3 -> 4 
            AddTransition(3, new RoomTransition
            {
                TargetRoomId = 4,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), // Right edge
                SpawnPosition = new Vector2(32, 160),
                Direction = TransitionDirection.Right
            });

            //Left exit 4 -> 3
            AddTransition(4, new RoomTransition
            {
                TargetRoomId = 3,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480), //Left edge
                SpawnPosition = new Vector2(976, 160),
                Direction = TransitionDirection.Left
            });

            //Right exit 4 -> 5
            AddTransition(4, new RoomTransition
            {
                TargetRoomId = 5,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), // Right edge
                SpawnPosition = new Vector2(32, 160),
                Direction = TransitionDirection.Right
            });

            //Left exit 5 -> 4
            AddTransition(5, new RoomTransition
            {
                TargetRoomId = 4,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480), //Left edge
                SpawnPosition = new Vector2(976, 160),
                Direction = TransitionDirection.Left
            });

            //Up exit 5 -> 6
            AddTransition(5, new RoomTransition
            {
                TargetRoomId = 6,
                TriggerArea = new Rectangle(640, 64, 32, 16),
                SpawnPosition = new Vector2(144, 144), 
                Direction = TransitionDirection.Up
            });

            //Down exit 6 -> 5
            AddTransition(6, new RoomTransition
            {
                TargetRoomId = 5,
                TriggerArea = new Rectangle(112, 128, EDGE_WIDTH, 480), 
                SpawnPosition = new Vector2(672, 144),
                Direction = TransitionDirection.Down
            });

            //Right exit 6 -> 7
            AddTransition(6, new RoomTransition
            {
                TargetRoomId = 7,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), // Right edge
                SpawnPosition = new Vector2(32, 160),
                Direction = TransitionDirection.Right
            });

            //Left exit 7 -> 6
            AddTransition(7, new RoomTransition
            {
                TargetRoomId = 6,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480), //Left edge
                SpawnPosition = new Vector2(976, 60),
                Direction = TransitionDirection.Left
            });

            //Down exit 4 -> 8
            AddTransition(4, new RoomTransition
            {
                TargetRoomId = 8,
                TriggerArea = new Rectangle(624, 160, 32, 16),
                SpawnPosition = new Vector2(944, 160),
                Direction = TransitionDirection.Down
            });

            //Up exit 8 -> 4
            AddTransition(8, new RoomTransition
            {
                TargetRoomId = 4,
                TriggerArea = new Rectangle(896, 112, 32, 16),
                SpawnPosition = new Vector2(624, 64),
                Direction = TransitionDirection.Up
            });

            //Left exit 8 -> 9
            AddTransition(8, new RoomTransition
            {
                TargetRoomId = 9,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480), //Left edge
                SpawnPosition = new Vector2(976, 160),
                Direction = TransitionDirection.Left
            });

            //Right exit 9 -> 8
            AddTransition(9, new RoomTransition
            {
                TargetRoomId = 8,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), // Right edge
                SpawnPosition = new Vector2(32, 160),
                Direction = TransitionDirection.Right
            });

            //Left exit 9 -> 10
            AddTransition(9, new RoomTransition
            {
                TargetRoomId = 10,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480), //Left edge
                SpawnPosition = new Vector2(976, 128),
                Direction = TransitionDirection.Left
            });

            //Right exit 10 -> 9
            AddTransition(10, new RoomTransition
            {
                TargetRoomId = 9,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), // Right edge
                SpawnPosition = new Vector2(32, 160),
                Direction = TransitionDirection.Right
            });

            //Down exit 5 -> 11
            AddTransition(5, new RoomTransition
            {
                TargetRoomId = 11,
                TriggerArea = new Rectangle(624, 176, 32, 16),
                SpawnPosition = new Vector2(64, 176),
                Direction = TransitionDirection.Down
            });


            //Up exit 11 -> 5
            AddTransition(11, new RoomTransition
            {
                TargetRoomId = 5,
                TriggerArea = new Rectangle(112, 112, 32, 16),
                SpawnPosition = new Vector2(576, 160),
                Direction = TransitionDirection.Up
            });


            //Right exit 11 -> 12
            AddTransition(11, new RoomTransition
            {
                TargetRoomId = 12,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), // Right edge
                SpawnPosition = new Vector2(32, 160),
                Direction = TransitionDirection.Right
            });

            //Left exit 12 -> 11
            AddTransition(12, new RoomTransition
            {
                TargetRoomId = 11,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480), //Left edge
                SpawnPosition = new Vector2(976, 60 ),
                Direction = TransitionDirection.Left
            });

            //Right exit 12 -> 13
            AddTransition(12, new RoomTransition
            {
                TargetRoomId = 13,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), // Right edge
                SpawnPosition = new Vector2(32, 160),
                Direction = TransitionDirection.Right
            });

            //Left exit 13 -> 12
            AddTransition(13, new RoomTransition
            {
                TargetRoomId = 12,
                TriggerArea = new Rectangle(0, 0, EDGE_WIDTH, 480), //Left edge
                SpawnPosition = new Vector2(976, 128),
                Direction = TransitionDirection.Left
            });

            //Right exit 13 -> Maze (14)
            AddTransition(13, new RoomTransition
            {
                TargetRoomId = 14,
                TriggerArea = new Rectangle(1024 - EDGE_WIDTH, 0, EDGE_WIDTH, 480), // Right edge
                SpawnPosition = new Vector2(768, 260),
                Direction = TransitionDirection.Right
            });






            //Maze -> Maze Room 1 (15)
            AddTransition(14, new RoomTransition
            {
                TargetRoomId = 15,
                TriggerArea = new Rectangle(80, 80, 16, 16), // Top edge
                SpawnPosition = new Vector2(32, 192),
                Direction = TransitionDirection.Up
            });

            //Maze Room 1  -> Maze
            AddTransition(15, new RoomTransition
            {
                TargetRoomId = 14,
                TriggerArea = new Rectangle(16, 16, 16, 32),
                SpawnPosition = new Vector2(96, 96),
                Direction = TransitionDirection.Down
            });

            //Maze -> Maze Room 2 (16)
            AddTransition(14, new RoomTransition
            {
                TargetRoomId = 16,
                TriggerArea = new Rectangle(640, 512, 16, 16), 
                SpawnPosition = new Vector2(32, 192),
                Direction = TransitionDirection.Down
            });

            //Maze Room 2 -> Maze
            AddTransition(16, new RoomTransition
            {
                TargetRoomId = 14,
                TriggerArea = new Rectangle(464, 16, 16, 32),
                SpawnPosition = new Vector2(624, 464),
                Direction = TransitionDirection.Down
            });

            //Maze -> Maze Room 3 (17)
            AddTransition(14, new RoomTransition
            {
                TargetRoomId = 17,
                TriggerArea = new Rectangle(368, 144, 16, 16),
                SpawnPosition = new Vector2(32, 192),
                Direction = TransitionDirection.Right
            });

            //Maze Room 3 -> Maze
            AddTransition(17, new RoomTransition
            {
                TargetRoomId = 14,
                TriggerArea = new Rectangle(512, 112, 16, 32),
                SpawnPosition = new Vector2(384, 176),
                Direction = TransitionDirection.Down
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