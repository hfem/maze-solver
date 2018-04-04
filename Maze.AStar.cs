
namespace MazeSolver
{
    public partial class Maze
    {
        struct AStar
        {
            private readonly int _pathLength;
            private readonly Tile _tile;
            
            public int PathLength { get => _pathLength; }
            public Tile Path { get => _tile; }

            public AStar( int pathLengthFromStart, Tile path )
            {
                _pathLength = pathLengthFromStart;
                _tile = path;
            }
        }

    }
}
