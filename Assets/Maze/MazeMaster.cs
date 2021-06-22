using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

// Handles maze -> has generator and renderer component and interconnects them
public class MazeMaster : MonoBehaviour
{
    // Singleton pattern
    public static MazeMaster Instance { get; private set; }

    [SerializeField]
    private int mazeWidth = 32;
    [SerializeField]
    private int mazeHeight = 18;
    [SerializeField]
    private Tilemap tilemap;
    [SerializeField]
    private List<MazeTile> tileData;

    private IMazeGenerator mazeGenerator;
    private IMazeRenderer mazeRenderer;
    private MazeTile[,] maze;


    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        // Generate and render maze
        mazeGenerator = new MazeGenerator(tileData);
        mazeRenderer = new MazeRenderer(tilemap);
        maze = mazeGenerator.GenerateMaze(mazeWidth, mazeHeight);
        mazeRenderer.RenderMaze(maze, mazeWidth, mazeHeight);
    }

    public void DestryPlatformsInRange(Vector3 position, float range)
	{
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeHeight; j++)
            {
                if (maze[i, j] == null)
                    continue;

                Vector3 tilePosition = new Vector3(i - mazeWidth / 2, j - mazeHeight / 2, 0);
                float distance = Vector3.Distance(position, tilePosition);

                if(distance < range)
                    maze[i, j] = null;
            }
        }

        mazeRenderer.RenderMaze(maze, mazeWidth, mazeHeight);
    }
}
