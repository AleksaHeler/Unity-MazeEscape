using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

// Describes a tile of the maze (for now only contains is it a wall or not)
[CreateAssetMenu]
public class MazeTile : ScriptableObject
{
	public TileBase tile;
	public bool isWall = false;
}
