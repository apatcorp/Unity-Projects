using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    public Maze[] mazes;
    int index = 0;

    List<Maze.Info> mazeObjects = new List<Maze.Info>();

    Maze.Info currentMaze;
    public Dictionary<int, MazeCell> mazeCellDictionary
    {
        get
        {
            return currentMaze.mazeCellDict;
        }
    }

    private static GameManager singleton = null;
    public static GameManager Singleton
    {
        get
        {
            return singleton;
        }
    }

    private void Awake()
    {
        if (singleton == null)
            singleton = this;
    }

    void Start ()
    {
        // generate all the mazes
        foreach (Maze maze in mazes)
        {
            mazeObjects.Add(maze.GenerateMaze(transform));
        }

        // set the first one visible
        currentMaze = mazeObjects[index];
        currentMaze.mazeObject.SetActive(true);

        // instantiate player
        InstantiatePlayer();
	}

    void InstantiatePlayer ()
    {
        // spawn the player at a corner cell of th maze
        Vector3[] possiblePositions = new Vector3[4];

        possiblePositions[0] = currentMaze.cells[0, 0].worldPosition;   // bottom left corner
        possiblePositions[1] = currentMaze.cells[0, currentMaze.height - 1].worldPosition;  // top left corner
        possiblePositions[2] = currentMaze.cells[currentMaze.width - 1, 0].worldPosition;  // top right corner
        possiblePositions[3] = currentMaze.cells[currentMaze.width - 1, currentMaze.height - 1].worldPosition;  // bottom right corner

        Vector3 spawnPosition = possiblePositions[Random.Range(0, possiblePositions.Length)];

        // create the player at position
        GameObject playerGO = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }
}
