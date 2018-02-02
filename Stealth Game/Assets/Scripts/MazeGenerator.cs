using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour {

    public Transform wallPrefab;
    public int width, height;
    public int scale = 1;

    public enum Algorithm { PRIM, KRUSKAL, RECURSIVE_BACKTRACKER }
    public Algorithm algorithm;

    Cell[,] cells;
	
	void Start ()
    {
        //GernerateMazePrim();
    }
	
	public void GernerateMazePrim (int seed)
    {
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cells[x, y] = new Cell(x, y);
            }
        }

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 position = new Vector3((-width / 2 + x) * scale, 0f, (-height / 2 + y) * scale);

                if (y < height && x < width)
                {
                    // create bottom wall
                    Transform bottomWall = CreateBottomWall(position);
                    Cell adjacentTopCell = (y < height - 1) ? cells[x, y] : null;
                    Cell adjacentBottomCell = (y > 0) ? cells[x, y - 1] : null;
                    cells[x, y].wallBottom = new Wall(adjacentTopCell, adjacentBottomCell, bottomWall);

                    // create left wall
                    Transform leftWall = CreateLeftWall(position);
                    Cell adjacentLeftCell = (x > 0) ? cells[x - 1, y] : null;
                    Cell adjacentRightCell = (x < width - 1) ? cells[x, y] : null;
                    cells[x, y].wallLeft = new Wall(adjacentLeftCell, adjacentRightCell, leftWall);

                    // set top and right wall
                    if (y > 0)
                    {
                        cells[x, y - 1].wallTop = cells[x, y].wallBottom;
                    }

                    if (x > 0)
                    {
                        cells[x - 1, y].wallRight = cells[x, y].wallLeft;
                    }
                }

                // set border walls
                if (y == height - 1)
                {
                    Transform topWall = CreateTopWall(position);
                    Cell adjacentTopCell = null;
                    Cell adjacentBottomCell = cells[x, y];
                    cells[x, y].wallTop = new Wall(adjacentTopCell, adjacentBottomCell, topWall);
                    cells[x, y].wallBottom.adjacentCell1 = cells[x, y];
                }

                if (x == width - 1)
                {
                    Transform wallRight = CreateRightWall(position);
                    Cell adjacentLeftCell = (x > 0) ? cells[x - 1, y] : null;
                    Cell adjacentRightCell = null;
                    cells[x, y].wallRight = new Wall(adjacentLeftCell, adjacentRightCell, wallRight);
                    cells[x, y].wallLeft.adjacentCell2 = cells[x, y];
                }

                // set neighbours
                if (x > 0 && y > 0 && x < width - 1 && y < height - 1)
                {
                    // top neighbor
                    cells[x, y].neighbours.Add(cells[x, y + 1]);

                    // bottom neighbor
                    cells[x, y].neighbours.Add(cells[x, y - 1]);

                    // left neighbor
                    cells[x, y].neighbours.Add(cells[x - 1, y]);

                    // right neighbor
                    cells[x, y].neighbours.Add(cells[x + 1, y]);
                }
                // left border cells (w/o corner)
                else if (x == 0 && y > 0 && y < height - 1)
                {
                    cells[x, y].neighbours.Add(cells[x, y + 1]);
                    cells[x, y].neighbours.Add(cells[x + 1, y]);
                    cells[x, y].neighbours.Add(cells[x, y - 1]);
                }
                // right border cells (w/o corner)
                else if (x == width - 1 && y > 0 && y < height - 1)
                {
                    cells[x, y].neighbours.Add(cells[x, y + 1]);
                    cells[x, y].neighbours.Add(cells[x - 1, y]);
                    cells[x, y].neighbours.Add(cells[x, y - 1]);
                }
                // bottom border cells (w/o corner)
                else if (x > 0 && x < height - 1 && y == 0)
                {
                    cells[x, y].neighbours.Add(cells[x + 1, y]);
                    cells[x, y].neighbours.Add(cells[x, y + 1]);
                    cells[x, y].neighbours.Add(cells[x - 1, y]);
                }
                // top border cells (w/o corner)
                else if (x > 0 && x < height - 1 && y == height - 1)
                {
                    cells[x, y].neighbours.Add(cells[x + 1, y]);
                    cells[x, y].neighbours.Add(cells[x, y - 1]);
                    cells[x, y].neighbours.Add(cells[x - 1, y]);
                }
                // corner left bottom
                else if (x == 0 && y == 0)
                {
                    cells[x, y].neighbours.Add(cells[x + 1, y]);
                    cells[x, y].neighbours.Add(cells[x, y + 1]);
                }
                // corner right bottom
                else if (x == width - 1 && y == 0)
                {
                    cells[x, y].neighbours.Add(cells[x - 1, y]);
                    cells[x, y].neighbours.Add(cells[x, y + 1]);
                }
                // corner left top
                else if (x == 0 && y == height - 1)
                {
                    cells[x, y].neighbours.Add(cells[x + 1, y]);
                    cells[x, y].neighbours.Add(cells[x, y - 1]);
                }
                // corner right top
                else if (x == width - 1 && y == height - 1)
                {
                    cells[x, y].neighbours.Add(cells[x - 1, y]);
                    cells[x, y].neighbours.Add(cells[x, y - 1]);
                }
            }
        }

        switch (algorithm)
        {
            case Algorithm.PRIM:
                PrimAlgorithm(seed);
                break;
            case Algorithm.KRUSKAL:
                RandomizedKruskal(seed);
                break;
            case Algorithm.RECURSIVE_BACKTRACKER:
                RecursiveBackTracker(seed);
                break;
        }

    }

    public void DestroyCurrentMaze ()
    {
        List<GameObject> objToDestroy = new List<GameObject>();

        foreach (Transform item in transform)
        {
            objToDestroy.Add(item.gameObject);
        }

        objToDestroy.ForEach(x => DestroyImmediate(x));

        cells = null;
    }

    void PrimAlgorithm (int seed)
    {
        print("Prim");

        // pick a random cell from the map
        Cell currentCell = cells[Random.Range(0, width - 1), Random.Range(0, height - 1)];

        // mark it as part of the maze
        currentCell.partOfMaze = true;

        // add all its walls to the wall list
        List<Wall> wallList = new List<Wall>() { currentCell.wallLeft, currentCell.wallRight, currentCell.wallBottom, currentCell.wallTop };

        // init pseudo random number generator with seed
        Random.InitState(seed);

        // perform algorithm aslong as there are walls in the wall list
        while (wallList.Count > 0)
        {
            // get a random wall from wall list
            Wall randomWall = wallList[Random.Range(0, wallList.Count)];

            // get unvisited cell
            Cell unvisitedCell = randomWall.GetNotVisitedAdjacentCell();
            if (unvisitedCell != null)
            {
                // make unvisited cell part of maze
                cells[unvisitedCell.indexX, unvisitedCell.indexY].partOfMaze = true;

                // add neighboring walls to the wall list
                if (cells[unvisitedCell.indexX, unvisitedCell.indexY].wallLeft != randomWall)
                {
                    wallList.Add(cells[unvisitedCell.indexX, unvisitedCell.indexY].wallLeft);
                }
                if (cells[unvisitedCell.indexX, unvisitedCell.indexY].wallRight != randomWall)
                {
                    wallList.Add(cells[unvisitedCell.indexX, unvisitedCell.indexY].wallRight);
                }
                if (cells[unvisitedCell.indexX, unvisitedCell.indexY].wallBottom != randomWall)
                {
                    wallList.Add(cells[unvisitedCell.indexX, unvisitedCell.indexY].wallBottom);
                }
                if (cells[unvisitedCell.indexX, unvisitedCell.indexY].wallTop != randomWall)
                {
                    wallList.Add(cells[unvisitedCell.indexX, unvisitedCell.indexY].wallTop);
                }

                // destroy wall object
                DestroyImmediate(randomWall.wall.gameObject);
            } 
            
            // remove the wall
            wallList.Remove(randomWall);
        }
    }

    public void RandomizedKruskal(int seed)
    {
        print("Kruskal");

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

                // add walls to wall list except border walls
                if (x > 0 && y > 0 && x < width - 1 && y < height - 1)
                {
                    wallList.Add(cells[x, y].wallLeft);
                    wallList.Add(cells[x, y].wallRight);
                    wallList.Add(cells[x, y].wallTop);
                    wallList.Add(cells[x, y].wallBottom);
                }
                // left and right border cells
                else if ((x == 0 && y > 0) || (x == width - 1 && y > 0))
                {
                    wallList.Add(cells[x, y].wallBottom);
                }
                // bottom border cells
                else if ((x > 0 && y == 0) || (x > 0 && y == height - 1))
                {
                    wallList.Add(cells[x, y].wallLeft);
                }
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

            // get the adjacent cells
            Cell cell1 = randomWall.adjacentCell1;
            Cell cell2 = randomWall.adjacentCell2;

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
                if (randomWall.wall.gameObject != null)
                    DestroyImmediate(randomWall.wall.gameObject);

                // join sets
                set1.UnionWith(set2);
                set2.Clear();
                cellHashSet.Remove(set2);
            }
            // remove wall from wall list
            wallList.Remove(randomWall);
        }
    }

    void RecursiveBackTracker(int seed)
    {
        print("Recursive Backtracker");

        // initial pseudo random number generator
        Random.InitState(seed);

        // calculate total amount of unvisited cells at the start
        int totalAmountOfCells = width * height;

        // set an initial cell
        Cell initialCell = cells[0, 0];

        // start backtracking
        RecursiveBacktracking(initialCell, totalAmountOfCells);
    }

    void RecursiveBacktracking (Cell initialCell, int totalCellsUnvisited)
    {
        // set current cell and set it as visited
        Cell currentCell = initialCell;
        currentCell.partOfMaze = true;
        
        while (totalCellsUnvisited > 0)
        {
            // local list of unvisited neighbours
            List<Cell> unvisitedNeighbours = new List<Cell>();

            for (int i = 0; i < currentCell.neighbours.Count; i++)
            {
                if (!currentCell.neighbours[i].partOfMaze)
                    unvisitedNeighbours.Add(currentCell.neighbours[i]);
            }

            // select neighbour
            if (unvisitedNeighbours.Count > 0)
            {
                Cell newCell = unvisitedNeighbours[Random.Range(0, unvisitedNeighbours.Count)];

                // remove wall between two cells
                Wall wallToDestroy = Cell.GetWallBetweenTwoCells(newCell, currentCell);
                
                if (wallToDestroy.wall != null)
                    DestroyImmediate(wallToDestroy.wall.gameObject);

                // go layer deeper
                RecursiveBacktracking(newCell, --totalCellsUnvisited);

            } else
            {
                // end recursion
                return;
                
            }
        }
        return;
    }


    Transform CreateLeftWall (Vector3 position)
    {
        Transform wallLeft = CreateWall();
        wallLeft.name = "Wall Left";
        wallLeft.position = new Vector3(position.x - scale / 2f, 0f, position.z);
        wallLeft.rotation = Quaternion.Euler(0f, 90, 0f);

        return wallLeft;
    }

    Transform CreateRightWall(Vector3 position)
    {
        Transform wallRight = CreateWall();
        wallRight.name = "Right Wall";
        wallRight.position = new Vector3(position.x + scale / 2f, 0f, position.z);
        wallRight.rotation = Quaternion.Euler(0f, 90, 0f);

        return wallRight;
    }

    Transform CreateTopWall(Vector3 position)
    {
        Transform wallTop = CreateWall();
        wallTop.name = "Top Wall";
        wallTop.position = new Vector3(position.x, 0f, position.z + scale / 2f);

        return wallTop;
    }

    Transform CreateBottomWall(Vector3 position)
    {
        Transform wallBottom = CreateWall();
        wallBottom.name = "Bottom Wall";
        wallBottom.position = new Vector3(position.x, 0f, position.z - scale / 2f);

        return wallBottom;
    }

    Transform CreateWall ()
    {
        Transform wall = Instantiate(wallPrefab, transform) as Transform;
        wall.localScale *= scale;

        return wall;
    }

    public class Cell
    {
        public Vector3 position;
        private Wall _wallLeft, _wallRight, _wallTop, _wallBottom;
        public Wall wallLeft { get { return _wallLeft; } set {  _wallLeft = value; } }
        public Wall wallRight { get { return _wallRight; } set { _wallRight = value; } }
        public Wall wallTop { get { return _wallTop; } set { _wallTop = value;  } }
        public Wall wallBottom { get { return _wallBottom; } set { _wallBottom = value; } }

        public bool partOfMaze;

        public List<Cell> neighbours = new List<Cell>();

        public int indexX, indexY;
        public int hashCode = 0;

        public Cell (int indexX, int indexY)
        {
            this.indexX = indexX;
            this.indexY = indexY;

            _wallLeft = null;
            _wallRight = null;
            _wallTop = null;
            _wallBottom = null;

            partOfMaze = false;
        }

        public static Wall GetWallBetweenTwoCells (Cell cell1, Cell cell2)
        {
            if (cell1.wallRight == cell2.wallLeft)
            {
                return cell1.wallRight;
            }
            if (cell1.wallLeft == cell2.wallRight)
            {
                return cell1.wallLeft;
            }
            if (cell1.wallTop == cell2.wallBottom)
            {
                return cell1.wallTop;
            }
            if (cell1.wallBottom == cell2.wallTop)
            {
                return cell1.wallBottom;
            }

            return null;
        }
    }

    public class Wall
    {
        public Cell adjacentCell1 { get; set; }
        public Cell adjacentCell2 { get; set; }
        public Transform wall { get; private set; }

        public Wall(Cell adjacentCell1, Cell adjacentCell2, Transform wall)
        {
            this.adjacentCell1 = adjacentCell1;
            this.adjacentCell2 = adjacentCell2;
            this.wall = wall;
        }

        public Cell GetNotVisitedAdjacentCell ()
        {
            // border walls
            if (adjacentCell1 == null || adjacentCell2 == null)
            {
                return null;
            }

            // four different wall scenarios
            if (!adjacentCell1.partOfMaze && !adjacentCell2.partOfMaze)
            {
                return null;
            }

            if (!adjacentCell1.partOfMaze && adjacentCell2.partOfMaze)
            {
                return adjacentCell1;
            }

            if (adjacentCell1.partOfMaze && !adjacentCell2.partOfMaze)
            {
                return adjacentCell2;
            }

            if (adjacentCell1.partOfMaze && adjacentCell2.partOfMaze)
            {
                return null;
            }

            return null;
        }
    }
}
