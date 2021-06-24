using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

// Displays given maze to the screen (puts it into tilemap system)
public class MazeRenderer : MonoBehaviour, IMazeRenderer
{
	private Tilemap tilemap;
	private int mazeWidth;
	private int mazeHeight;
	private ParticleSystem tileExplosionParticles;
	private List<Vector3> itemPositions;
	private int excessSpaceAroundMaze;
	private Transform itemsParent;


	public MazeRenderer(Tilemap tilemap, ParticleSystem tileExplosionParticles, int excessSpaceAroundMaze)
	{
		this.tilemap = tilemap;
		this.tileExplosionParticles = tileExplosionParticles;
		this.excessSpaceAroundMaze = excessSpaceAroundMaze;
		itemPositions = new List<Vector3>();
		itemsParent = new GameObject("Items").transform;
	}

	public void RenderMaze(MazeTile[,] maze, int mazeWidth, int mazeHeight)
	{
		this.mazeWidth = mazeWidth;
		this.mazeHeight = mazeHeight;

		TileBase wallTile = null;

		foreach (MazeTile tile in maze)
		{
			if (tile == null)
				continue;
			if (tile.isWall == true)
			{
				wallTile = tile.tile;
				break;
			}
		}

		for (int x = -excessSpaceAroundMaze; x < mazeWidth + excessSpaceAroundMaze; x++)
		{
			for (int y = -excessSpaceAroundMaze; y < mazeHeight + excessSpaceAroundMaze; y++)
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
				if (tileData != null)
					tilemap.SetTile(position, tileData.tile);
				else
					tilemap.SetTile(position, null);
			}
		}
	}

	public void ClearMaze()
	{
		// Set all tiles to null
		for (int x = -excessSpaceAroundMaze; x < mazeWidth + excessSpaceAroundMaze; x++)
		{
			for (int y = -excessSpaceAroundMaze; y < mazeHeight + excessSpaceAroundMaze; y++)
			{
				Vector3Int position = new Vector3Int(x - mazeWidth / 2, y - mazeHeight / 2, 0);
				tilemap.SetTile(position, null);
			}
		}

		// Destroy all items
		Destroy(itemsParent.gameObject);
		itemsParent = new GameObject("Items").transform;
	}

	public void SpawnItems(List<Item> items)
	{
		foreach (Item item in items)
		{
			SpawnItem(item);
		}
	}

	private void SpawnItem(Item item)
	{
		int count = Random.Range(item.minCount, item.maxCount);
		for (int i = 0; i < count; i++)
		{
			SpawnAtRandomPosition(item.itemPrefab);
		}
	}

	private void SpawnAtRandomPosition(GameObject item)
	{
		// First find position where there is no wall
		Vector3Int position = Vector3Int.zero;
		do
		{
			int x = Random.Range(0, mazeWidth);
			int y = Random.Range(0, mazeHeight);
			position = new Vector3Int(x - mazeWidth / 2, y - mazeHeight / 2, 0);

			// If we already spawned an item here, just ignore
			if (itemPositions.Contains(position) || position == Vector3Int.zero)
				continue;
		} while (tilemap.GetTile(position) != null);

		Instantiate(item, position, Quaternion.identity, itemsParent);
		itemPositions.Add(position);
	}

	public void DestroyPlatformsInRange(Vector3 position, float range)
	{
		for (int i = 0; i < mazeWidth; i++)
		{
			for (int j = 0; j < mazeHeight; j++)
			{
				// Find distance from given position to currently observed platform
				Vector3Int tilePosition = new Vector3Int(i - mazeWidth / 2, j - mazeHeight / 2, 0);
				float distance = Vector3.Distance(position, tilePosition);

				if (distance < range)
				{
					if (tilemap.GetTile(tilePosition) != null)
						Instantiate(tileExplosionParticles, tilePosition, Quaternion.identity);
					tilemap.SetTile(tilePosition, null);
				}
			}
		}
	}

}
