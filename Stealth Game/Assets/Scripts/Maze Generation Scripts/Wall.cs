using System.Collections.Generic;
using UnityEngine;

public class Wall {

    public List<Cell> adjacentCells;

    public Vector3 position;
    public bool horizontalWall = false;

    public Wall (Vector3 position, bool horizontalWall, params Cell[] adjacentCells)
    {
        this.position = position;
        this.adjacentCells = new List<Cell>(2);
        this.horizontalWall = horizontalWall;

        if (this.adjacentCells.Count < 2)
        {
            for (int i = 0; i < adjacentCells.Length; i++)
            {
                this.adjacentCells.Add(adjacentCells[i]);
            }
        }
    }

    public Cell GetNotVisitedAdjacentCell ()
    {
        Cell result = null;

        if (adjacentCells.Count == 2)
        {
            if (adjacentCells[0].visited && !adjacentCells[1].visited)
                result = adjacentCells[1];

            if (adjacentCells[1].visited && !adjacentCells[0].visited)
                result = adjacentCells[0];
        }

        return result;
    }
}
