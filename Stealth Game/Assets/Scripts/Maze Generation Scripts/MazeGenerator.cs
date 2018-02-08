using System.Collections.Generic;
using UnityEngine;

public static class MazeGenerator
{
    public enum Algorithm { PRIM, KRUSKAL, RECURSIVE_BACKTRACKER }
 
    public static Cell[,] GernerateMazeGrid (int width, int height, int cellWidth, Algorithm algorithm, int seed)
    {
        // get cell grid
        Cell [,] cells = GridGenerator.CreateCellGrid(width, height, cellWidth);

        // apply maze generation algorithm to the cell grid
        switch (algorithm)
        {
            case Algorithm.PRIM:
                PrimAlgorithm(ref cells, width, height, seed);
                break;
            case Algorithm.KRUSKAL:
                RandomizedKruskal(ref cells, seed);
                break;
            case Algorithm.RECURSIVE_BACKTRACKER:
                RecursiveBackTracker(ref cells, width, height, seed);
                break;
        }

        return cells;
    }

    static void PrimAlgorithm (ref Cell[,] cells, int width, int height, int seed)
    {
        // pick a random cell from the map
        Cell currentCell = cells[Random.Range(0, width - 1), Random.Range(0, height - 1)];

        // mark it as visited
        currentCell.visited = true;

        // add all its walls to the wall list
        List<Wall> wallList = new List<Wall>(currentCell.GetWalls());

        // init pseudo random number generator with seed
        Random.InitState(seed);

        // perform algorithm aslong as there are walls in the wall list
        while (wallList.Count > 0)
        {
            // get a random wall from the wall list
            Wall randomWall = wallList[Random.Range(0, wallList.Count)];

            // get not visited adjacent cell if possible
            Cell adjacentUnvisitedCell = randomWall.GetNotVisitedAdjacentCell();

            // check, whether a passage between the two cells this random wall seperates can be created
            if (adjacentUnvisitedCell != null)
            {
                // set the unvisited cell as visited
                cells[adjacentUnvisitedCell.x, adjacentUnvisitedCell.y].visited = true;

                // make the wall a passage (remove the wall from both cells)
                Cell cell1 = randomWall.adjacentCells[0];
                Cell cell2 = randomWall.adjacentCells[1];
                cells[cell1.x, cell1.y].RemoveWall(randomWall);
                cells[cell2.x, cell2.y].RemoveWall(randomWall);

                // add neighboring walls of the unvisited cell to the wall list
                wallList.AddRange(cells[adjacentUnvisitedCell.x, adjacentUnvisitedCell.y].GetWalls());
            } 
            
            // remove the wall
            wallList.Remove(randomWall);
        }
    }

    static void RandomizedKruskal(ref Cell[,] cells, int seed)
    {
        // create a list of all walls
        List<Wall> wallList = new List<Wall>();
        // create a list with a list for each cell
        List<HashSet<Cell>> cellHashSet = new List<HashSet<Cell>>();

        for (int x = 0; x < cells.GetLength(0); x++)
        {
            for (int y = 0; y < cells.GetLength(1); y++)
            {
                // add each cell to its own list in the cell set
                cellHashSet.Add(new HashSet<Cell>() { cells[x, y] });

                // add walls of the cell to wall list
                wallList.Add(cells[x, y].LeftWall);
                wallList.Add(cells[x, y].RightWall);
                wallList.Add(cells[x, y].TopWall);
                wallList.Add(cells[x, y].BottomWall);
            }
        }

        // initial pseudo random number generator
        Random.InitState(seed);

        // cache for temporary hash sets
        HashSet<Cell> set1 = new HashSet<Cell>();
        HashSet<Cell> set2 = new HashSet<Cell>();

        while (wallList.Count > 0)
        {
            // get a random wall from the wall list
            Wall randomWall = wallList[Random.Range(0, wallList.Count)];

            // only perform algorithm, when the wall is not a border wall
            if (randomWall.adjacentCells.Count == 2)
            {
                // get the adjacent cells
                Cell cell1 = randomWall.adjacentCells[0];
                Cell cell2 = randomWall.adjacentCells[1];

                foreach (HashSet<Cell> item in cellHashSet)
                {
                    if (item.Contains(cell1))
                    {
                        set1 = item;
                    }

                    if (item.Contains(cell2))
                    {
                        set2 = item;
                    }
                }

                // in different sets
                if (set1.GetHashCode() != set2.GetHashCode())
                {
                    // remove wall        
                    cells[cell1.x, cell1.y].RemoveWall(randomWall);
                    cells[cell2.x, cell2.y].RemoveWall(randomWall);

                    // join sets
                    set1.UnionWith(set2);
                    set2.Clear();
                    cellHashSet.Remove(set2);
                }
            }
            
            // remove wall from wall list
            wallList.Remove(randomWall);
        }
    }
    
    static void RecursiveBackTracker(ref Cell[,] cells, int width, int height, int seed)
    {
        // initial pseudo random number generator
        Random.InitState(seed);

        // calculate total amount of unvisited cells at the start
        int totalAmountOfCells = width * height;

        // set an initial cell
        Cell initialCell = cells[Random.Range(0, width), Random.Range(0, height)];

        // start backtracking
        RecursiveBacktracking(ref cells, initialCell, totalAmountOfCells);
    }

    static void RecursiveBacktracking (ref Cell[,] cells, Cell initialCell, int totalCellsUnvisited)
    {
        // set current cell and set it as visited
        Cell currentCell = initialCell;
        currentCell.visited = true;
        
        while (totalCellsUnvisited > 0)
        {
            // local list of unvisited neighbours
            List<Cell> unvisitedNeighbours = new List<Cell>();

            for (int i = 0; i < currentCell.GetNeighbours().Count; i++)
            {
                if (!currentCell.GetNeighbours()[i].visited)
                    unvisitedNeighbours.Add(currentCell.GetNeighbours()[i]);
            }

            // select neighbour
            if (unvisitedNeighbours.Count > 0)
            {
                // pick random unvisited neighbour
                Cell newCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];

                // remove wall between two cells
                Cell.RemoveWallBetweenTwoCells(ref currentCell, ref newCell);

                // go layer deeper
                RecursiveBacktracking(ref cells, newCell, --totalCellsUnvisited);

            } else
            {
                // end recursion
                return;               
            }
        }
        return;
    }
}