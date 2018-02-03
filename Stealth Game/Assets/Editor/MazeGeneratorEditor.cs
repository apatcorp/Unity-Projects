using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MazeGenerator))]
public class MazeGeneratorEditor : Editor {

    MazeGenerator mazeGenerator;

    private void OnEnable()
    {
        mazeGenerator = (MazeGenerator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Maze"))
        {
            mazeGenerator.DestroyCurrentMaze();
            mazeGenerator.GernerateMazeGrid(Random.Range(0, 10000));
        }

        if (GUILayout.Button("Destroy Maze"))
        {
            mazeGenerator.DestroyCurrentMaze();
        }
    }
}
