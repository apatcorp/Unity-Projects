using UnityEngine;

public static class WallCreator {

	public static void CreateWallsForMaze (Cell[,] maze, Transform parent, Transform wallPrefab)
    {
        int width = maze.GetLength(0);
        int height = maze.GetLength(1);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                // get cell
                Cell cell = maze[x, y];

                if (cell.LeftWall != null)
                {
                    Transform wallLeft = GameObject.Instantiate(wallPrefab, parent) as Transform;
                    wallLeft.transform.position = cell.LeftWall.position;
                    wallLeft.localScale = wallLeft.localScale * cell.cellWidth;
                    wallLeft.localScale += new Vector3(0.99f * wallLeft.localScale.z, 0.99f * wallLeft.localScale.z, 0f); 
                    wallLeft.rotation = Quaternion.Euler(0f, 90f, 0f);
                }

                if (cell.BottomWall != null)
                {
                    Transform wallBottom = GameObject.Instantiate(wallPrefab, parent) as Transform;
                    wallBottom.transform.position = cell.BottomWall.position;
                    wallBottom.localScale = wallBottom.localScale * cell.cellWidth;
                    wallBottom.localScale += new Vector3(0.99f * wallBottom.localScale.z, 0.99f * wallBottom.localScale.z, 0f);
                }

                if (cell.RightWall != null && x == width - 1)
                {
                    Transform wallRight = GameObject.Instantiate(wallPrefab, parent) as Transform;
                    wallRight.transform.position = cell.RightWall.position;
                    wallRight.localScale = wallRight.localScale * cell.cellWidth;
                    wallRight.localScale += new Vector3(0.99f * wallRight.localScale.z, 0.99f * wallRight.localScale.z, 0f);
                    wallRight.rotation = Quaternion.Euler(0f, 90f, 0f);
                }

                if (cell.TopWall != null && y == height - 1)
                {
                    Transform wallTop = GameObject.Instantiate(wallPrefab, parent) as Transform;
                    wallTop.transform.position = cell.TopWall.position;
                    wallTop.localScale = wallTop.localScale * cell.cellWidth;
                    wallTop.localScale += new Vector3(0.99f * wallTop.localScale.z, 0.99f * wallTop.localScale.z, 0f);
                }
            }
        }
    }
}
