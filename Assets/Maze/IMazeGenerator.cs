using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IMazeGenerator
{
	MazeTile[,] GenerateMaze(int mazeWidth, int mazeHeight);
}
