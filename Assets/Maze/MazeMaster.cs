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

    public void DestroyPlatformsInRange(Vector3 position, float range)
	{
        mazeRenderer.DestroyPlatformsInRange(position, range);
    }
}
