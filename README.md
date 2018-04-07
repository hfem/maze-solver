## Solving a Maze Using A\*

Solving a maze given an image with an overhead view of the maze is a popular graph theory problem. Running a shortest path algorithm on the graph built from the maze determines whether the start and finish tiles are connected (is there a solution to this maze) and determines the shortest path between these two tiles (the fastest path through the maze).  

**Problem Description:**  
Given an image of a maze where the start is marked by red tiles, the finish is marked by blue tiles, valid moves are limited to the 4 adjacent pixels, and the pathways through the maze are enclosed by black tiles which represent walls that cannot be traveled through, the graph looks like this: 

<pre><b>Graph G: (V , E) = (!black tiles , adjacency in image location)</b></pre>

*maze-solver* uses **A\*** to find the shortest path through this graph. **A\*** is an informed search algorithm; fundamentally a breadth first search modified with the use of a heuristic (a cost assigned to each move) to 'inform' the guess for next best move. The heuristic used in *maze-solver* was the **weight** of a specific vertex in the path,     
<pre><b>weight = # of vertices visited on the path until this vertex
         + straight line (Manhattan) distance to the goal coordinates</b>
</pre>

This heuristic will not overestimate the cost of each path it considers because travel on the graph is limited to straight line movement (movement to one of the four adjacent tiles). If cost is overestimated in the A\* algorithm, the solution found would not be guaranteed to be the most efficient, for this case that might  mean the path found would be longer than necessary. 

## Implementation
- **AStar** struct : *Maze.AStar.cs*
  - holds length of the path from the start tile to this maze tile && link to Tile
  - used in Maze for the A* priority queue: *key* = **weight**, *value* = list of AStar structs with Tiles that all have the same weight
  - nested class of Maze 
- **Coordinates** struct : *Maze.Coordinates.cs*
  - immutable value type, used as a dictionary key in Maze and as data in Tile class
  - keeps record of the physical pixel location that the Tile represents
  - nested class of Maze
- **Tile** class : *Maze.Tile.cs*
  - a node for the graph of the maze
  - holds a location in the image, state of the tile [start, path, finish] derived from the color, and a 'breadcrumb' link to a tile that is next in an A* path, and possible moves that can be made next from this tile
  - nested class of Maze
- **Maze** class : *Maze.cs*
  - builds the graph out of Tile nodes and runs the A* algorithm on this graph
  - Build () - checks color of pixel and if !black then adds to the dictionary of tiles, checks for the two tile neighbors that would already be in the graph if they are valid moves and adds links 
  - Solve () - runs A* on graph using a dictionary sorted by weight and a list of AStar structs to determine what move to evaluate next
  - Print () -  makes a copy of the maze image and colors the solution path green
- **Program** class : *MazeSolver.cs*
  - builds and solves the maze based on the image input
  - user input functions here for the maze image file
