using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Maze  {

    public Transform wallPrefab;
    public GameObject mazeCellPrefab;

    [Range(1, 50)]
    public int width = 10, height = 10;
    [Range(1, 10)]
    public int cellWidth = 5;

    public MazeGenerator.Algorithm algorithm;

    [Range(1, 1000)]
    public int seed = 20;

    [Range(2, 8)]
    public int lightDensity = 2;

    public Material floorMaterial;
    public Material ceilingMaterial;

    public Info GenerateMaze (Transform parent)
    {
        // create a gamee object that holds the walls
        GameObject mazeHolder = new GameObject("Maze(" + System.Enum.GetName(typeof(MazeGenerator.Algorithm), algorithm) + ")");
        mazeHolder.transform.SetParent(parent);
        mazeHolder.transform.position = Vector3.zero;

        // create cells
        Cell[,] cells = MazeGenerator.GernerateMazeGrid(width, height, cellWidth, algorithm, seed);

        // create walls
        WallCreator.CreateWallsForMaze(cells, mazeHolder.transform, wallPrefab);

        // create floor and ceiling of the maze
        CreateFloor(mazeHolder.transform);
        CreateCeiling(mazeHolder.transform);

        // set it invisibel by default
        mazeHolder.SetActive(false);

        // create a new maze info object
        Info mazeInfo = new Info(mazeHolder, cells, width, height, cellWidth);

        // create lights in the maze
        CreateLights(ref mazeInfo, mazeHolder.transform);

        return mazeInfo;
    }

    void CreateFloor (Transform mazeHolder)
    {
        // create a bottom plane
        GameObject floor = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor.name = "Floor";
        floor.transform.SetParent(mazeHolder);
        floor.transform.localPosition = -new Vector3(cellWidth / 2f, -0.2f * cellWidth, cellWidth / 2f);
        floor.transform.localScale = new Vector3(width, .02f, height) * cellWidth;
        
        if (floorMaterial != null)
            floor.GetComponent<MeshRenderer>().material = floorMaterial;
    }

    void CreateCeiling (Transform mazeHolder)
    {
        // create a top plane
        GameObject ceiling = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ceiling.name = "Ceiling";
        ceiling.transform.SetParent(mazeHolder);
        ceiling.transform.localPosition = -new Vector3(cellWidth / 2f, -cellWidth + 0.1f, cellWidth / 2f);
        ceiling.transform.localScale = new Vector3(width, .1f, height) * cellWidth;

        if (ceilingMaterial != null)
            ceiling.GetComponent<MeshRenderer>().material = ceilingMaterial;
    }

    void CreateLights (ref Info mazeInfo, Transform mazeHolder)
    {
        for (int y = 0; y < mazeInfo.height; y++)
        {
            for (int x = 0; x < mazeInfo.width; x++)
            {
                // create maze cell
                GameObject mazeCell = GameObject.Instantiate(mazeCellPrefab, mazeHolder);
                mazeCell.name = "MazeCell(" + x + ", " + y + ")";

                MazeCell mazeCellComp = mazeCell.GetComponent<MazeCell>();
                // enable lights at specific conditions
                mazeCellComp.SetupMazeCell(mazeInfo.cells[x, y], (x % lightDensity == 0));
            }
        }
    }

    public class Info
    {
        public GameObject mazeObject { get; private set; }
        public Cell[,] cells { get; private set; }

        public int width { get; private set; }
        public int height { get; private set; }
        public int cellWidth { get; private set; }

        public Info(GameObject mazeObject, Cell[,] cells, int width, int height, int cellWidth)
        {
            this.mazeObject = mazeObject;
            this.cells = cells;
            this.width = width;
            this.height = height;
            this.cellWidth = cellWidth;
        }
    }
}
