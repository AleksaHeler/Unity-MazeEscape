using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Generates a maze with given values and returns maze tile array -> [,] 2D array
public class MazeGenerator : IMazeGenerator
{
	private List<MazeTile> tileData;
	private MazeTile[,] maze;
	private MazeTile wallTile;
	private int mazeWidth;
	private int mazeHeight;

	public MazeGenerator(List<MazeTile> tileData)
	{
		this.tileData = tileData;

		// Find a tile to use as wall and path
		wallTile = tileData.Find(tile => tile.isWall == true);
	}

	public MazeTile[,] GenerateMaze(int mazeWidth, int mazeHeight)
	{
		this.mazeWidth = mazeWidth;
		this.mazeHeight = mazeHeight;

		// Create array
		maze = new MazeTile[mazeWidth, mazeHeight];

		// Set all tiles to wall type
		for (int i = 0; i < mazeWidth; i++)
		{
			for (int j = 0; j < mazeHeight; j++)
			{
				maze[i, j] = wallTile;
			}
		}

		// Start recursive maze generation
		RecursiveMazeGeneration(mazeWidth / 2, mazeHeight / 2);

		return maze;
	}

	private void RecursiveMazeGeneration(int x, int y)
	{
		// Mark this as visited (set as path)
		maze[x, y] = null;


		// While it has any unvisited neighbours
		while (true)
		{
			List<Vector2Int> unvisitedNeighbours = new List<Vector2Int>();
			FindAllNeighbours(unvisitedNeighbours, x, y);

			// If no neighbours found we are done
			if(unvisitedNeighbours.Count == 0)
				break;

			// Get random neighbour
			int randomIndex = Random.Range(0, unvisitedNeighbours.Count);
			Vector2Int neighbour = unvisitedNeighbours[randomIndex];

			// Remove the wall between them
			int offsetX = (neighbour.x - x) / 2;
			int offsetY = (neighbour.y - y) / 2;
			maze[x + offsetX, y + offsetY] = null;

			// Invoke the routine recursively
			RecursiveMazeGeneration(neighbour.x, neighbour.y);
		}
	}

	private void FindAllNeighbours(List<Vector2Int> unvisitedNeighbours, int x, int y)
	{
		unvisitedNeighbours.Clear();

		if (x - 2 > 0 && maze[x - 2, y] == wallTile)
			unvisitedNeighbours.Add(new Vector2Int(x - 2, y));
		if (x + 2 < mazeWidth - 1 && maze[x + 2, y] == wallTile)
			unvisitedNeighbours.Add(new Vector2Int(x + 2, y));
		if (y + 2 < mazeHeight - 1 && maze[x, y + 2] == wallTile)
			unvisitedNeighbours.Add(new Vector2Int(x, y + 2));
		if (y - 2 > 0 && maze[x, y - 2] == wallTile)
			unvisitedNeighbours.Add(new Vector2Int(x, y - 2));
	}
}
