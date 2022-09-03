using System.Collections.Generic;
using System;

using Microsoft.Xna.Framework;

using DungianoDesktop.Components.Characters;
using DungianoDesktop.Components.Objects;
using DungianoDesktop.Components.Scenes;

namespace DungianoDesktop.Components.Map
{
    public class LevelBuilder
    {
        private DungianoGame _dungianoGame;
        private (int Width, int Height) _screenSize;
        private (int Width, int Height) _roomSize;

        private RoomBank _roomBank;

        private Random _random;
        private LevelScene _scene;

        private Vector2 _enemySpawn;

        public LevelBuilder(DungianoGame dungianoGame, LevelScene scene)
        {
            _dungianoGame = dungianoGame;
            _screenSize = dungianoGame.GetScreenSize();
            _roomSize = (_screenSize.Width, (_screenSize.Height * 8) / 9);
            _scene = scene;

            _roomBank = new RoomBank(_screenSize);
            _random = new Random();

            _enemySpawn = new Vector2(_roomSize.Width / 2, _roomSize.Height / 2);
        }

        public List<Room> BuildMap(int level)
        {
            List<Room> map = new List<Room>();
            map.Add(_generateHall(level));

            for (int i = 0; i < level + 3; i++)
            {
                map.Add(_generateRoom(i + 1, level));
            }

            return map;
        }

        public Player CreatePlayer()
        {
            return new Player(_scene, _dungianoGame, new Vector2(_screenSize.Width/2, _screenSize.Height/2));
        }

        public Vector2 GetPlayerSpawnPosition()
        {
            return new Vector2(_roomSize.Width / 2, _roomSize.Height - 100);
        }

        private Room _generateHall(int level)
        {
            List<Body> doors = new List<Body>();

            _createHallDoor(doors, level);

            return new Room(new Background(_dungianoGame, _roomBank.GetRoomAt(0).TextureName), _roomBank.GetRoomAt(0).Walls, doors) ;
        }

        private void _createHallDoor(List<Body> doors, int level)
        {
            List<int> numbers = new List<int>();

            for (int j = 0; j < level + 3; j++)
            {
                numbers.Add(j + 1);
            }

            _shuffle(numbers);

            int i = 0;

            while (i < level + 3)
            {

                if (i < 4)
                    doors.Add(new Door(_dungianoGame, ("Objects/doorClose", "Objects/doorOpen"), new Vector2(12, (_roomSize.Height / 5) * (4 - i)), (float)Math.PI * 3 / 2, numbers[i]));
                else if (i < 10)
                    doors.Add(new Door(_dungianoGame, ("Objects/doorClose", "Objects/doorOpen"), new Vector2((_roomSize.Width / 7) * (i - 3), 12), 0f, numbers[i]));
                else if (i < 13)
                    doors.Add(new Door(_dungianoGame, ("Objects/doorClose", "Objects/doorOpen"), new Vector2(_roomSize.Width - 12, (_roomSize.Height / 5) * (i - 9)), (float)Math.PI / 2, numbers[i]));
                i++;
            }

            doors.Add(new FinalDoor(_dungianoGame, new Vector2(_screenSize.Width - 12, _roomSize.Height / 5 * 4)));

        }

        //uses Fisher and Yates algorithm
        private void _shuffle(List<int> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int i = _random.Next(n + 1);
                int temp = list[i];
                list[i] = list[n];
                list[n] = temp;
            }
        }

        private Room _generateRoom(int numberOfRoom, int level)
        {
            int room = _random.Next(1, _roomBank.GetSize());

            Background background = new Background(_dungianoGame, _roomBank.GetRoomAt(room).TextureName);
            List<Body> entities = new List<Body>();
            // here add enemies to the list

            entities.Add(new Door(_dungianoGame, ("Objects/doorClose", _roomBank.GetRoomAt(room).DoorTextureName), new Vector2(_roomSize.Width / 2, 12), 0f, 0, false));

            if (numberOfRoom == level + 3)
                _summonEnemies(entities, numberOfRoom, level, true);
            else
                _summonEnemies(entities, numberOfRoom, level, false);

            return new Room(background, _roomBank.GetRoomAt(room).Walls, entities);
        }

        private void _summonEnemies(List<Body> entities, int numberOfRoom, int level, bool lastRoom)
        {
            int summonScore = numberOfRoom * level;
            int numEnemy = 0;
            CleanerItem item;

            while (summonScore > 0)
            {
                item = CleanerItem.Empty;
                if (numEnemy == 0)
                {
                    if (lastRoom)
                        item = CleanerItem.GoldKey;
                    else
                        item = CleanerItem.GreyKey;
                }
                else if (numEnemy == 2)
                    item = CleanerItem.Gramophone;
                else if (numEnemy == 3 && lastRoom)
                    item = CleanerItem.Weapon;


                if (item == CleanerItem.Weapon)
                {
                    if (summonScore > 20)
                    {
                        entities.Add(new FollowingCleaner(_dungianoGame, _scene, _enemySpawn, _scene.WeaponBank.GetWeaponAt(level)));
                        summonScore -= 20;
                    }
                    else if (summonScore > 5)
                    {
                        entities.Add(new FollowingCleaner(_dungianoGame, _scene, _enemySpawn, _scene.WeaponBank.GetWeaponAt(level)));
                        summonScore -= 5;
                    }
                    else
                    {
                        entities.Add(new Cleaner(_dungianoGame, _scene, _enemySpawn, _scene.WeaponBank.GetWeaponAt(level)));
                        summonScore -= 1;
                    }
                }
                else
                {
                    if (summonScore > 20)
                    {
                        entities.Add(new FollowingCleaner(_dungianoGame, _scene, _enemySpawn, item));
                        summonScore -= 20;
                    }
                    else if (summonScore > 5)
                    {
                        entities.Add(new FatCleaner(_dungianoGame, _scene, _enemySpawn, item));
                        summonScore -= 5;
                    }
                    else
                    {
                        entities.Add(new Cleaner(_dungianoGame, _scene, _enemySpawn, item));
                        summonScore -= 1;
                    }
                }

                numEnemy += 1;
            }
        }
    }

    public struct RoomScratch
    {
        public string TextureName;
        public string DoorTextureName;
        public List<Rectangle> Walls;

        public RoomScratch(string TextureName, string DoorTextureName, List<Rectangle> Walls)
        {
            this.TextureName = TextureName;
            this.DoorTextureName = DoorTextureName;
            this.Walls = Walls;
        }
    }

    public class RoomBank
    {
        List<RoomScratch> rooms = new List<RoomScratch>();

        private (int Width, int Height) _screenSize;

        public RoomBank((int, int) screenSize)
        {
            _screenSize = screenSize;
            _initializeRooms();
        }

        private void _initializeRooms()
        {
            rooms.Add(new RoomScratch("Rooms/hall", "Objects/doorOpen", _createHallWalls()));
            rooms.Add(new RoomScratch("Rooms/r1", "Objects/doorOpenR1", _createHallWalls()));
            rooms.Add(new RoomScratch("Rooms/r2", "Objects/doorOpenR2", _createHallWalls()));
            rooms.Add(new RoomScratch("Rooms/r3", "Objects/doorOpenR3", _createHallWalls()));
        }

        private List<Rectangle> _createHallWalls()
        {
            int wallThickness = 25;
            int inventoryHeight = _screenSize.Height / 9;

            List<Rectangle> walls = new List<Rectangle>();
            walls.Add(new Rectangle(0, 0, _screenSize.Width, wallThickness));
            walls.Add(new Rectangle(0, 0, wallThickness, _screenSize.Height - inventoryHeight));
            walls.Add(new Rectangle(0, _screenSize.Height - wallThickness - inventoryHeight, _screenSize.Width, wallThickness));
            walls.Add(new Rectangle(_screenSize.Width - wallThickness, 0, wallThickness, _screenSize.Height - inventoryHeight));

            // add flowers
            int flowerSize = 50;

            walls.Add(new Rectangle(wallThickness, wallThickness, flowerSize, flowerSize));
            walls.Add(new Rectangle(wallThickness, _screenSize.Height - wallThickness - flowerSize, flowerSize, flowerSize));
            walls.Add(new Rectangle(_screenSize.Width - wallThickness - flowerSize, _screenSize.Height - wallThickness - flowerSize, flowerSize, flowerSize));
            walls.Add(new Rectangle(_screenSize.Width - wallThickness - flowerSize, wallThickness, flowerSize, flowerSize));

            return walls ;
        }

        public RoomScratch GetRoomAt(int index)
        {
            return rooms[index];
        }

        public int GetSize()
        {
            return rooms.Count;
        }
    }
}

