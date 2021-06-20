using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

// Displays given maze to the screen (puts it into tilemap system)
public class MazeRenderer : IMazeRenderer
{
	private Tilemap tilemap;

	public MazeRenderer(Tilemap tilemap)
	{
		this.tilemap = tilemap;
	}

	public void RenderMaze(MazeTile[,] maze, int mazeWidth, int mazeHeight)
	{
		for(int x = 0; x < mazeWidth; x++)
		{
			for(int y = 0; y < mazeHeight; y++)
			{
				// Calculate position
				Vector3Int position = new Vector3Int(x - mazeWidth / 2, y - mazeHeight / 2, 0);
				MazeTile tileData = maze[x, y];

				// Set tile
				tilemap.SetTile(position, tileData.tile);
			}
		}
	}
}
