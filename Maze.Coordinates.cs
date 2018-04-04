using System;

namespace MazeSolver
{
    public partial class Maze
    {
        struct Coordinates : IEquatable<Coordinates>
        {
            private readonly int _x;
            private readonly int _y;

            public int X { get => _x; }
            public int Y { get => _y; }

            public Coordinates(int cx, int cy)
            {
                _x = cx;
                _y = cy;
            }

            public bool Equals(Coordinates point)
            {
                return (_x == point.X) && (_y == point.Y);
            }
            
            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 17;
                    hash = (hash * 1572869) ^ _x;
                    hash = (hash * 786433) ^ _y;
                    return hash;
                }
            }
        }
    }
}