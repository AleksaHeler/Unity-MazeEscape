using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using System;

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
    [SerializeField]
    private ParticleSystem tileExplosionParticles;
    [SerializeField]
    private List<Item> items;
    [SerializeField]
	private int excessSpaceAroundMaze = 50;

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
        mazeRenderer = new MazeRenderer(tilemap, tileExplosionParticles, excessSpaceAroundMaze);
        maze = mazeGenerator.GenerateMaze(mazeWidth, mazeHeight);
        mazeRenderer.RenderMaze(maze, mazeWidth, mazeHeight);
        mazeRenderer.SpawnItems(items);

        PlayerAbility.OnLevelChanged += OnLevelChangedCallback;
    }

	private void OnLevelChangedCallback()
	{
        maze = mazeGenerator.GenerateMaze(mazeWidth, mazeHeight);
        mazeRenderer.ClearMaze();
        mazeRenderer.RenderMaze(maze, mazeWidth, mazeHeight);
        mazeRenderer.SpawnItems(items);
    }

    public void DestroyPlatformsInRange(Vector3 position, float range)
	{
        mazeRenderer.DestroyPlatformsInRange(position, range);
    }
}
