using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public List<Maze> mazes;
    int currentMazeIndex = 0;

    List<Maze.Info> mazeObjects = new List<Maze.Info>();

    Maze.Info currentMaze;

    GameManager gameManager;

    void Awake ()
    {
        CreateMazes();
	}

    void Start()
    {
        gameManager = GameManager.Singleton;

        // set the first one visible
        currentMaze = mazeObjects[currentMazeIndex++];
        currentMaze.mazeObject.SetActive(true);

        SpawnPlayer();
    }

    void CreateMazes ()
    {
        // generate all the mazes
        foreach (Maze maze in mazes)
        {
            mazeObjects.Add(maze.GenerateMaze(transform));
        }
    }

    void SpawnPlayer()
    {
        // spawn the player at a corner cell of the maze
        Vector3[] possiblePlayerSpawnPositions = new Vector3[4];

        possiblePlayerSpawnPositions[0] = currentMaze.cells[0, 0].worldPosition;   // bottom left corner
        possiblePlayerSpawnPositions[1] = currentMaze.cells[0, currentMaze.height - 1].worldPosition;  // top left corner
        possiblePlayerSpawnPositions[2] = currentMaze.cells[currentMaze.width - 1, 0].worldPosition;  // top right corner
        possiblePlayerSpawnPositions[3] = currentMaze.cells[currentMaze.width - 1, currentMaze.height - 1].worldPosition;  // bottom right corner

        Vector3 spawnPosition = possiblePlayerSpawnPositions[Random.Range(0, possiblePlayerSpawnPositions.Length)];

        // create the player at position
        gameManager.Spawner.SpawnPlayer(spawnPosition, Quaternion.identity);
    }
}
