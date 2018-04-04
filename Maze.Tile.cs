using System.Collections.Generic;

namespace MazeSolver
{
    public partial class Maze
    {
        class Tile
        {
            // tiles that are adjacent && state == Path
            private readonly List<Tile> _possibleMoves;

            // pixel location of the tile
            private readonly Coordinates _location;

            // Start, Path, Finish
            private readonly TileState _state;

            // set after A* has run && tile is on a path
            private Tile _breadcrumb;


            public List<Tile> PossibleMoves { get => _possibleMoves; }
            public Coordinates Location { get => _location; }
            public TileState State { get => _state; }
            public Tile Breadcrumb
            {
                get => _breadcrumb;
                set => _breadcrumb = value;
            }

            // constructors
            public Tile( Coordinates location, TileState state )
            {
                _location = location;
                _state = state;
                _possibleMoves = new List<Tile>(4);
                _breadcrumb = null;
            }
        }
    }
}
