using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

// Displays given maze to the screen (puts it into tilemap system)
public class MazeRenderer : MonoBehaviour, IMazeRenderer
{
	private Tilemap tilemap;

	public MazeRenderer(Tilemap tilemap)
	{
		this.tilemap = tilemap;
	}

	public void RenderMaze(MazeTile[,] maze, int mazeWidth, int mazeHeight)
	{
		TileBase wallTile = null;
		foreach(MazeTile tile in maze)
		{
			if (tile == null)
				continue;
			if(tile.isWall == true)
			{
				wallTile = tile.tile;
				break;
			}
		}

		for (int x = -30; x < mazeWidth + 30; x++)
		{
			for(int y = -30; y < mazeHeight + 30; y++)
			{
				// Calculate position
				Vector3Int position = new Vector3Int(x - mazeWidth / 2, y - mazeHeight / 2, 0);

				if (x < 0 || x >= mazeWidth || y < 0 || y >= mazeHeight)
				{
					tilemap.SetTile(position, wallTile);
					continue;
				}

				// Set tile
				MazeTile tileData = maze[x, y];
				if(tileData != null)
					tilemap.SetTile(position, tileData.tile);
				else
					tilemap.SetTile(position, null);
			}
		}
	}
}
