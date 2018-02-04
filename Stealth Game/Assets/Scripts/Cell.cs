using System.Collections.Generic;
using UnityEngine;

public class Cell {

    public int x, y;
    public int cellWidth;

    Wall topWall;
    public Wall TopWall
    {
        get
        {
            return topWall;
        }
        set
        {
            if (value == null)
            {
                if (topWall != null)
                    walls.Remove(topWall);
            }
            else
            {
                if (!walls.Contains(value))
                    walls.Add(value);
            }
            topWall = value;
        }
    }
    Wall bottomWall;
    public Wall BottomWall
    {
        get
        {
            return bottomWall;
        }
        set
        {
            if (value == null)
            {
                if (bottomWall != null)
                    walls.Remove(bottomWall);
            }
            else
            {
                if (!walls.Contains(value))
                    walls.Add(value);
            }
            bottomWall = value;
        }
    }
    Wall leftWall;
    public Wall LeftWall
    {
        get
        {
            return leftWall;
        }
        set
        {
            if (value == null)
            {
                if (leftWall != null)
                    walls.Remove(leftWall);
            }
            else
            {
                if (!walls.Contains(value))
                    walls.Add(value);
            }
            leftWall = value;
        }
    }
    Wall rightWall;
    public Wall RightWall
    {
        get
        {
            return rightWall;
        }
        set
        {
            if (value == null)
            {
                if (rightWall != null)
                    walls.Remove(rightWall);
            }
            else
            {
                if (!walls.Contains(value))
                    walls.Add(value);
            }
            rightWall = value;
        }
    }

    Cell leftNeighbourCell;
    public Cell LeftNeighbourCell
    {
        get
        {
            return leftNeighbourCell;
        }

        set
        {
            if (value == null)
            {
                if (leftNeighbourCell != null)
                    neighbours.Remove(leftNeighbourCell);
            }
            else
            {
                if (!neighbours.Contains(value))
                    neighbours.Add(value);
            }
            leftNeighbourCell = value;
        }
    }

    Cell rightNeighbourCell;
    public Cell RightNeighbourCell
    {
        get
        {
            return rightNeighbourCell;
        }

        set
        {
            if (value == null)
            {
                if (rightNeighbourCell != null)
                    neighbours.Remove(rightNeighbourCell);
            }
            else
            {
                if (!neighbours.Contains(value))
                    neighbours.Add(value);
            }
            rightNeighbourCell = value;
        }
    }

    Cell topNeighbourCell;
    public Cell TopNeighbourCell
    {
        get
        {
            return topNeighbourCell;
        }

        set
        {
            if (value == null)
            {
                if (topNeighbourCell != null)
                    neighbours.Remove(topNeighbourCell);
            }
            else
            {
                if (!neighbours.Contains(value))
                    neighbours.Add(value);
            }
            topNeighbourCell = value;
        }
    }

    Cell bottomNeighbourCell;
    public Cell BottomNeighbourCell
    {
        get
        {
            return bottomNeighbourCell;
        }

        set
        {
            if (value == null)
            {
                if (bottomNeighbourCell != null)
                    neighbours.Remove(bottomNeighbourCell);
            }
            else
            {
                if (!neighbours.Contains(value))
                    neighbours.Add(value);
            }
            bottomNeighbourCell = value;
        }
    }

    List<Cell> neighbours;
    List<Wall> walls;

    public Vector3 worldPosition;

    public bool visited;

    public Cell(int x, int y, Vector3 worldPosition, int cellWidth)
    {
        this.x = x;
        this.y = y;
        this.worldPosition = worldPosition;
        this.cellWidth = cellWidth;

        visited = false;

        neighbours = new List<Cell>(4);
        walls = new List<Wall>(4);
    }

    public List<Cell> GetNeighbours ()
    {
        return neighbours;
    }

    public List<Wall> GetWalls()
    {
        return walls;
    }

    public void RemoveWall (Wall wall)
    {
        if (wall == leftWall)
            LeftWall = null;

        else if (wall == rightWall)
            RightWall = null;

        else if (wall == topWall)
            TopWall = null;

        else if (wall == bottomWall)
            BottomWall = null;
    }

    public static void RemoveWallBetweenTwoCells (ref Cell cell1, ref Cell cell2)
    {
        if (cell1.RightWall == cell2.LeftWall)
        {
            cell1.RightWall = null;
            cell2.LeftWall = null;
        }
        else if (cell2.RightWall == cell1.LeftWall)
        {
            cell2.RightWall = null;
            cell1.LeftWall = null;
        }
        else if (cell2.TopWall == cell1.BottomWall)
        {
            cell1.BottomWall = null;
            cell2.TopWall = null;
        }
        else if (cell2.BottomWall == cell1.TopWall)
        {
            cell2.BottomWall = null;
            cell1.TopWall = null;
        }
    }
}
