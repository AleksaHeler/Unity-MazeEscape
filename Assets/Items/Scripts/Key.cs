using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IItem
{
	public void PickUp()
	{
		Debug.Log("I am being picked up");
		Destroy(gameObject);
	}
}
