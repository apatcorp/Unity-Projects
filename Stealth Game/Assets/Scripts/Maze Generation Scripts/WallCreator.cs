using UnityEngine;

public static class WallCreator {

	public static void CreateWallsForMaze (Cell[,] maze, Transform parent, Transform wallPrefab)
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        GameObject wallHolder = new GameObject("Walls");
        wallHolder.transform.SetParent(parent);
        wallHolder.transform.position = Vector3.zero;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // get cell
                Cell cell = maze[x, y];

                if (cell.LeftWall != null)
                {
                    Transform wallLeft = GameObject.Instantiate(wallPrefab, wallHolder.transform) as Transform;
                    wallLeft.transform.position = cell.LeftWall.position;
                    wallLeft.localScale = wallLeft.localScale * cell.cellWidth;
                    wallLeft.rotation = Quaternion.Euler(0f, 90f, 0f);
                }

                if (cell.BottomWall != null)
                {
                    Transform wallBottom = GameObject.Instantiate(wallPrefab, wallHolder.transform) as Transform;
                    wallBottom.transform.position = cell.BottomWall.position;
                    wallBottom.localScale = wallBottom.localScale * cell.cellWidth;
                }

                if (cell.RightWall != null && x == width - 1)
                {
                    Transform wallRight = GameObject.Instantiate(wallPrefab, wallHolder.transform) as Transform;
                    wallRight.transform.position = cell.RightWall.position;
                    wallRight.localScale = wallRight.localScale * cell.cellWidth;
                    wallRight.rotation = Quaternion.Euler(0f, 90f, 0f);
                }

                if (cell.TopWall != null && y == height - 1)
                {
                    Transform wallTop = GameObject.Instantiate(wallPrefab, wallHolder.transform) as Transform;
                    wallTop.transform.position = cell.TopWall.position;
                    wallTop.localScale = wallTop.localScale * cell.cellWidth;
                }
            }
        }
    }
}
