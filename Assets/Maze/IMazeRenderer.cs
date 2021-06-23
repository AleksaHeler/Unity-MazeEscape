using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IMazeRenderer 
{
	void RenderMaze(MazeTile[,] maze, int mazeWidth, int mazeHeight);
	void DestroyPlatformsInRange(Vector3 position, float range);
}
