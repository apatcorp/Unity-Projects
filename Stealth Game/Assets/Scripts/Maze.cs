using UnityEngine;

[System.Serializable]
public class Maze  {

    public Transform wallPrefab;
    [Range(1, 50)]
    public int width, height;
    [Range(1, 5)]
    public int cellWidth = 1;

    public MazeGenerator.Algorithm algorithm;

    [Range(1, 1000)]
    public int seed = 20;

    public Material groundMaterial;


    public Info GenerateMaze (Transform parent)
    {
        // create a gamee object that holds the walls
        GameObject mazeHolder = new GameObject("Maze(" + System.Enum.GetName(typeof(MazeGenerator.Algorithm), algorithm) + ")");
        mazeHolder.transform.SetParent(parent);
        mazeHolder.transform.position = Vector3.zero;

        // create cells
        Cell[,] cells = MazeGenerator.GernerateMazeGrid(width, height, cellWidth, algorithm, seed);
;
        // create walls
        WallCreator.CreateWallsForMaze(cells, mazeHolder.transform, wallPrefab);

        // create a plane
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.SetParent(mazeHolder.transform);
        plane.transform.localPosition = -new Vector3(cellWidth / 2f, 0f, cellWidth / 2f);
        plane.transform.localScale = new Vector3(width / 10f, 1, height / 10f) * cellWidth;

        if (groundMaterial != null)
            plane.GetComponent<MeshRenderer>().material = groundMaterial;

        // set it invisibel by default
        mazeHolder.SetActive(false);

        // create a new maze info object
        Info mazeInfo = new Info(mazeHolder, cells, width, height, cellWidth);

        return mazeInfo;
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
