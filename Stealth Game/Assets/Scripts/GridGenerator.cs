using UnityEngine;

public static class GridGenerator {

	public static Cell[,] CreateCellGrid (int width, int height, int cellWidth)
    {
        Cell[,] cellGrid = new Cell[width, height];

        // create a cell grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // create a cell
                cellGrid[x, y] = new Cell(x, y, cellWidth);
            }
        }

        // set the neighbours for each cell in the grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // corner cells
                bool cornerCell = (x == 0 && y == 0) || (x == 0 && y == height - 1) || (x == width - 1 && y == 0) || (x == width - 1 && y == height - 1);

                // border cells
                bool borderCell = (x == 0 && y > 0 && y < height - 1) || (x == width - 1 && y > 0 && y < height - 1) || (x > 0 && x < width - 1 && y == 0) || (x > 0 && x < width - 1 && y == height - 1);

                if (cornerCell)
                {
                    cellGrid[x, y] = SetNeighboursForCornerCell(cellGrid, cellGrid[x, y], width, height);
                } 
                else if (borderCell)
                {
                    cellGrid[x, y] = SetNeighboursForBorderCell(cellGrid, cellGrid[x, y], width, height);
                }
                else
                {
                    cellGrid[x, y] = SetNeighboursForCentralCell(cellGrid, cellGrid[x, y], width, height);
                }
            }
        }

        // set the wall for each cell in the cell grid
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // position of the cell in world space
                Vector3 cellPosition = new Vector3(-width / 2f + x * cellWidth , 0f, -height / 2f + y * cellWidth);

                // create left and bottom wall
                Wall leftWall = new Wall(new Vector3(cellPosition.x - cellWidth / 2f, 0f, cellPosition.z), false, cellGrid[x, y]);
                Wall bottomWall = new Wall(new Vector3(cellPosition.x, 0f, cellPosition.z - cellWidth / 2f), true, cellGrid[x, y]);

                // set the corresponding walls in the current cell
                cellGrid[x, y].LeftWall = leftWall;
                cellGrid[x, y].BottomWall = bottomWall;

                if (x > 0)
                {
                    // add a neighboutring cell to the current left wall
                    leftWall.adjacentCells.Add(cellGrid[x - 1, y]);
                    // set the right wall of the left neighbour of that cell
                    cellGrid[x, y].LeftNeighbourCell.RightWall = leftWall;
                }

                if (y > 0)
                {
                    // add a neighboutring cell to the current bottom wall
                    bottomWall.adjacentCells.Add(cellGrid[x, y - 1]);
                    // set the top wall of the bottom neighbour of that cell
                    cellGrid[x, y].BottomNeighbourCell.TopWall = bottomWall;
                }

                if (x == width - 1)
                {
                    Wall rightWall = new Wall(new Vector3(cellPosition.x + cellWidth / 2f, 0f, cellPosition.z), false, cellGrid[x, y]);
                    cellGrid[x, y].RightWall = rightWall;
                }

                if (y == height - 1)
                {
                    Wall topWall = new Wall(new Vector3(cellPosition.x, 0f, cellPosition.z + cellWidth / 2f), true, cellGrid[x, y]);
                    cellGrid[x, y].TopWall = topWall;
                }
            }
        }

        return cellGrid;
    }

    static Cell SetNeighboursForCornerCell (Cell[,] cellGrid, Cell cornerCell, int width, int height)
    {
        // corner left bottom
        if (cornerCell.x == 0 && cornerCell.y == 0)
        {
            cornerCell.RightNeighbourCell = cellGrid[cornerCell.x + 1, cornerCell.y];   // right neighbour
            cornerCell.TopNeighbourCell = cellGrid[cornerCell.x, cornerCell.y + 1];     // top neighbour
        }
        // corner right bottom
        if (cornerCell.x == width - 1 && cornerCell.y == 0)
        {
            cornerCell.LeftNeighbourCell = (cellGrid[cornerCell.x - 1, cornerCell.y]);     // left neighbour
            cornerCell.TopNeighbourCell = (cellGrid[cornerCell.x, cornerCell.y + 1]);     // top neighbour
        }
        // corner left top
        if (cornerCell.x == 0 && cornerCell.y == height - 1)
        {
            cornerCell.RightNeighbourCell = (cellGrid[cornerCell.x + 1, cornerCell.y]);     // right neighbour
            cornerCell.BottomNeighbourCell = (cellGrid[cornerCell.x, cornerCell.y - 1]);     // bottom neighbour
        }
        // corner right top
        if (cornerCell.x == width - 1 && cornerCell.y == height - 1)
        {
            cornerCell.LeftNeighbourCell = (cellGrid[cornerCell.x - 1, cornerCell.y]);       // left neighbour
            cornerCell.BottomNeighbourCell = (cellGrid[cornerCell.x, cornerCell.y - 1]);     // bottom neighbour
        }

        return cornerCell;
    }

    static Cell SetNeighboursForBorderCell (Cell[,] cellGrid, Cell borderCell, int width, int height)
    {
        // left border
        if (borderCell.x == 0)
        {
            borderCell.TopNeighbourCell = (cellGrid[borderCell.x, borderCell.y + 1]);       // top neighbour
            borderCell.RightNeighbourCell = (cellGrid[borderCell.x + 1, borderCell.y]);     // right neighbour
            borderCell.BottomNeighbourCell = (cellGrid[borderCell.x, borderCell.y - 1]);     // bottom neighbour
        }
        // right border
        if (borderCell.x == width - 1)
        {
            borderCell.TopNeighbourCell = (cellGrid[borderCell.x, borderCell.y + 1]);       // top neighbour
            borderCell.LeftNeighbourCell = (cellGrid[borderCell.x - 1, borderCell.y]);     // left neighbour
            borderCell.BottomNeighbourCell = (cellGrid[borderCell.x, borderCell.y - 1]);     // bottom neighbour
        }
        // top border
        if (borderCell.y == height - 1)
        {
            borderCell.LeftNeighbourCell = (cellGrid[borderCell.x - 1, borderCell.y]);       // left neighbour
            borderCell.BottomNeighbourCell = (cellGrid[borderCell.x, borderCell.y - 1]);     // bottom neighbour
            borderCell.RightNeighbourCell = (cellGrid[borderCell.x + 1, borderCell.y]);     // right neighbour
        }
        // bottom border
        if (borderCell.y == 0)
        {
            borderCell.LeftNeighbourCell = (cellGrid[borderCell.x - 1, borderCell.y]);       // left neighbour
            borderCell.TopNeighbourCell = (cellGrid[borderCell.x, borderCell.y + 1]);     // top neighbour
            borderCell.RightNeighbourCell = (cellGrid[borderCell.x + 1, borderCell.y]);     // right neighbour
        }

        return borderCell;
    }

    static Cell SetNeighboursForCentralCell(Cell[,] cellGrid, Cell centralCell, int width, int height)
    {
        // add all four neighbours
        centralCell.LeftNeighbourCell = (cellGrid[centralCell.x - 1, centralCell.y]);     // left
        centralCell.RightNeighbourCell = (cellGrid[centralCell.x + 1, centralCell.y]);     // right
        centralCell.TopNeighbourCell = (cellGrid[centralCell.x, centralCell.y + 1]);     // top
        centralCell.BottomNeighbourCell = (cellGrid[centralCell.x, centralCell.y - 1]);     // bottom

        return centralCell;
    }
}