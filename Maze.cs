using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;

namespace MazeSolver
{
    public partial class Maze
    {
        private Bitmap _mazeImage;
        private Dictionary<Coordinates,Tile> _pathways;

        private Coordinates _start;
        private Coordinates _goal;

        private Tile _shortestPath;
        
        // enum for type of maze tile 
        enum TileState { Start, Path, Finish };

        // constructors
        public Maze()
        {
            _pathways = new Dictionary<Coordinates, Tile>();
        }

        public Maze(Bitmap maze) {
            this._mazeImage = maze;
            _pathways = new Dictionary<Coordinates, Tile>();
        }

        // methods
        public void Build()
        {
            Coordinates up, left, currentXnY;
            Tile current, canMove;
            bool startSet = false, goalSet = false;
            Color pixelColor;
            
            for (int x = 0, width = _mazeImage.Width; x < width; x++)
            {
                for (int y = 0, height = _mazeImage.Height; y < height; y++)
                {
                    pixelColor = _mazeImage.GetPixel(x, y);

                    if ((pixelColor.R != 0) ||
                        (pixelColor.G != 0) ||
                        (pixelColor.B != 0)) // != Black
                    {
                        currentXnY = new Coordinates(x, y);
                        
                        // determine the state of the tile { Start, Path, Finish }                       
                        if ((pixelColor.R == 255) &&
                            (pixelColor.G == 0)   &&
                            (pixelColor.B == 0)) // == Red
                        {
                            current = new Tile(currentXnY, TileState.Start);
                            if(!startSet)
                            {
                                _start = currentXnY;
                                startSet = true;
                            }
                        }
                        else if ((pixelColor.R == 0) &&
                                 (pixelColor.G == 0) &&
                                 (pixelColor.B == 255)) // == Blue
                        {
                            current = new Tile(currentXnY, TileState.Finish);
                            if (!goalSet)
                            {
                                _goal = currentXnY;
                                goalSet = true;
                            }
                        }
                        else
                        {
                            current = new Tile(currentXnY, TileState.Path);
                        }
                        
                        // check if tile above is a valid move
                        up = new Coordinates(x, y - 1);
                        if (_pathways.TryGetValue(up, out canMove))
                        {
                            current.PossibleMoves.Add(canMove);
                            canMove.PossibleMoves.Add(current);
                        }

                        // check if tile to the left is a valid move
                        left = new Coordinates(x - 1, y);
                        if (_pathways.TryGetValue(left, out canMove))
                        {
                            current.PossibleMoves.Add(canMove);
                            canMove.PossibleMoves.Add(current);
                        }
                        
                        _pathways.Add(currentXnY, current);
                    }
                }
            }
        }

        public void Solve()
        {
            int goalX = _goal.X;
            int goalY = _goal.Y;
            Tile start = _pathways[_start];

            // Priority Queue : priority for A* 'weight' of tiles
            SortedDictionary<int, List<AStar>> sortedPathWeights;
            KeyValuePair<int, List<AStar>> lowestWeight;
            List<AStar> sameWeights, temp;
            int distance, pathLength, weight;
            AStar current;
            bool foundGoal = false;

            // initialize A* metrics
            sortedPathWeights = new SortedDictionary<int, List<AStar>>();
            distance = Math.Abs(goalX - _start.X) + Math.Abs(goalY - _start.Y); 
            pathLength = 1;
            weight = pathLength + distance;
            current = new AStar(pathLength, start);
            start.Breadcrumb = start;

            sameWeights = new List<AStar>() { current };
            sortedPathWeights.Add(weight, sameWeights);

            while (!foundGoal)
            {
                try
                {
                    lowestWeight = sortedPathWeights.First();
                }
                catch (InvalidOperationException)
                {
                    Console.WriteLine("Sorry, this maze doesn't have a solution.");
                    throw;
                }
                
                sortedPathWeights.Remove(lowestWeight.Key);
                sameWeights = lowestWeight.Value;

                foreach (AStar i in sameWeights)
                { // go through all Tiles with the same A* weight

                    foreach (Tile j in i.Path.PossibleMoves)
                    { // calculate A* for all posible moves

                        if (j.State == TileState.Finish)
                        {
                            foundGoal = true;
                            j.Breadcrumb = i.Path;
                            _shortestPath = j;
                            start.Breadcrumb = null;
                            break;
                        }

                        // if visited, continue to next tile
                        if (j.Breadcrumb != null) { continue; }

                        j.Breadcrumb = i.Path;
                        distance = Math.Abs(goalX - j.Location.X)
                                + Math.Abs(goalY - j.Location.Y);
                        pathLength = i.PathLength + 1;
                        weight = pathLength + distance;
                        current = new AStar(pathLength, j);

                        if (sortedPathWeights.TryGetValue(weight, out temp))
                        {
                            temp.Add(current);
                        }
                        else
                        {
                            temp = new List<AStar>();
                            temp.Add(current);
                            sortedPathWeights.Add(weight, temp);
                        }
                    }
                }

            }         
        }

        public Bitmap Print()
        {
            Bitmap solvedMaze = new Bitmap(_mazeImage);
            Color green = Color.FromArgb(0, 255, 0);


            for ( Tile i = _shortestPath; i.State != TileState.Start; i = i.Breadcrumb)
            {
                solvedMaze.SetPixel(i.Location.X, i.Location.Y, green);
            }

            return solvedMaze;
        }
    }

    
}
