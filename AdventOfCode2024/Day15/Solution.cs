using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdventOfCode2024.Utilities;

namespace AdventOfCode2024.Day15
{
    public enum MapObject
    {
        WALL,
        BOX,
        EMPTY,
        PLAYER,
        BOX_LEFT,
        BOX_RIGHT,
        UNKNOWN
    }

    public enum Instruct
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
        UNKNOWN
    }

    public class Solution : ISolution
    {
        private Regex _mapRegex = new Regex(@"[#.O@]+");
        private Regex _instructRegex = new Regex(@"[<>^v]+");

        private readonly Dictionary<Instruct, Point> DirectionOffsets = new()
        {
            { Instruct.UP, new Point(0, -1) },
            { Instruct.DOWN, new Point(0, 1) },
            { Instruct.RIGHT, new Point(1, 0) },
            { Instruct.LEFT, new Point(-1, 0) },
        };

        private readonly Dictionary<Instruct, char> InstuctionMap = new()
        {
            { Instruct.UP, '^' },
            { Instruct.DOWN, 'v' },
            { Instruct.LEFT, '<' },
            { Instruct.RIGHT, '>' },
        };

        private readonly Dictionary<char, Instruct> InstuctionCharMap = new()
        {
            { '^', Instruct.UP  },
            { 'v', Instruct.DOWN },
            { '<', Instruct.LEFT },
            { '>', Instruct.RIGHT },
        };

        private readonly Dictionary<MapObject, char> MapObjectMap = new()
        {
            { MapObject.WALL, '#' },
            { MapObject.BOX, 'O' },
            { MapObject.PLAYER, '@' },
            { MapObject.EMPTY, '.' },
            { MapObject.BOX_LEFT, '[' },
            { MapObject.BOX_RIGHT, ']' },
        };

        private readonly Dictionary<char, MapObject> MapObjectCharMap = new()
        {
            { '#', MapObject.WALL  },
            { 'O', MapObject.BOX },
            { '@', MapObject.PLAYER },
            { '.', MapObject.EMPTY },
            { '[', MapObject.BOX_LEFT },
            { ']', MapObject.BOX_RIGHT },
        };

        public void SolvePart1(string[] inputData)
        {
            ParseInput(inputData, out var map, out var instructions);

            Point playerPosition = new Point();

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == MapObject.PLAYER)
                    {
                        playerPosition = new Point(x, y);
                    }
                }
            }

            foreach (var inst in instructions)
            {
                if (CanMovePart1(playerPosition, map, inst, out var firstEmptyPosition) && firstEmptyPosition.HasValue)
                {
                    MoveMapObjectPart1(playerPosition, map, inst, firstEmptyPosition.Value);
                    playerPosition += DirectionOffsets[inst];
                }
            }

            Console.WriteLine($"Box Position Total: {GetBoxSum(map)}");
        }

        public void SolvePart2(string[] inputData)
        {
            ParseInput(inputData, out var map, out var instructions);
            var extendedMap = ExtendMap(map);
            PrintMap(extendedMap);
            var canMove = CanMovePart2(new Point(6, 4), extendedMap, Instruct.UP);
            return;
            
            Point playerPosition = new Point();

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == MapObject.PLAYER)
                    {
                        playerPosition = new Point(x, y);
                    }
                }
            }

            foreach (var inst in instructions)
            {
                //TryMove(playerPosition, map, inst, out playerPosition);
            }

            Console.WriteLine($"Box Position Total: {GetBoxSum(map)}");
        }

        public void ParseInput(string[] inputData, out MapObject[][] map, out Instruct[] instructions)
        {
            List<MapObject[]> tempMap = new List<MapObject[]>();
            List<Instruct> tempInstructions = new List<Instruct>();

            foreach (string line in inputData)
            {
                if (string.IsNullOrEmpty(line)) continue;

                var match = _mapRegex.Match(line);
                if (match.Success)
                {
                    tempMap.Add(match.Groups[0].Value.Trim().Select(c => MapObjectCharMap[c]).ToArray());
                    continue;
                }

                match = _instructRegex.Match(line);
                if (match.Success)
                {
                    tempInstructions.AddRange(match.Groups[0].Value.Trim().Select(c => InstuctionCharMap[c]));
                }
            }

            map = tempMap.ToArray();
            instructions = tempInstructions.ToArray();
        }

        public MapObject[][] ExtendMap(MapObject[][] map)
        {
            MapObject[][] extendedMap = new MapObject[map.Length][];

            for (int y = 0; y < map.Length; y++)
            {
                extendedMap[y] = new MapObject[map[y].Length * 2];
                for (int x = 0; x < map[y].Length; x++)
                {
                    int extendedX = x * 2;
                    switch (map[y][x])
                    {
                        case MapObject.WALL:
                            extendedMap[y][extendedX] = MapObject.WALL;
                            extendedMap[y][extendedX + 1] = MapObject.WALL;
                            break;
                        case MapObject.EMPTY:
                            extendedMap[y][extendedX] = MapObject.EMPTY;
                            extendedMap[y][extendedX + 1] = MapObject.EMPTY;
                            break;
                        case MapObject.BOX:
                            extendedMap[y][extendedX] = MapObject.BOX_LEFT;
                            extendedMap[y][extendedX + 1] = MapObject.BOX_RIGHT;
                            break;
                        case MapObject.PLAYER:
                            extendedMap[y][extendedX] = MapObject.PLAYER;
                            extendedMap[y][extendedX + 1] = MapObject.EMPTY;
                            break;
                    }
                }
            }

            return extendedMap;
        }

        public void PrintMap(MapObject[][] map)
        {
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    Console.Write(MapObjectMap[map[y][x]]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public bool CanMovePart1(Point position, MapObject[][] map, Instruct direction, out Point? firstEmptyPosition)
        {
            Point movePos = position + DirectionOffsets[direction];
            MapObject moveObject = map[movePos.Y][movePos.X];

            while (moveObject != MapObject.WALL && moveObject != MapObject.EMPTY)
            {
                movePos += DirectionOffsets[direction];
                moveObject = map[movePos.Y][movePos.X];
            }

            firstEmptyPosition = moveObject == MapObject.EMPTY ? movePos : null;
            return moveObject == MapObject.EMPTY;
        }

        public bool CanMovePart2(Point position, MapObject[][] map, Instruct direction)
        {
            MapObject currObject = map[position.Y][position.X];

            if (currObject == MapObject.WALL) return false;
            if (currObject == MapObject.EMPTY) return true;

            if (direction == Instruct.LEFT ||  direction == Instruct.RIGHT) return CanMovePart2(position + DirectionOffsets[direction], map, direction);

            if (currObject == MapObject.BOX_LEFT)
            {
                return CanMovePart2(position + DirectionOffsets[direction], map, direction) &&
                    CanMovePart2(position + DirectionOffsets[Instruct.RIGHT] + DirectionOffsets[direction], map, direction);
            }
            else
            {
                return CanMovePart2(position + DirectionOffsets[direction], map, direction) &&
                    CanMovePart2(position + DirectionOffsets[Instruct.LEFT] + DirectionOffsets[direction], map, direction);
            }
        }

        public void MoveMapObjectPart1(Point startPoint, MapObject[][] map, Instruct direction, Point emptyPositionInDirection)
        {
            Point currPoint = emptyPositionInDirection;
            MapObject currObject = map[currPoint.Y][currPoint.X];

            while (!currPoint.Equals(startPoint))
            {
                Point swapPoint = currPoint - DirectionOffsets[direction];
                MapObject swapObject = map[swapPoint.Y][swapPoint.X];

                map[currPoint.Y][currPoint.X] = swapObject;
                map[swapPoint.Y][swapPoint.X] = currObject;

                currPoint = swapPoint;
            }
        }

        public int GetBoxSum(MapObject[][] map)
        {
            int total = 0;
            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == MapObject.BOX)
                    {
                        total += (100 * y) + x;
                    }
                }
            }
            return total;
        }
    }
}
