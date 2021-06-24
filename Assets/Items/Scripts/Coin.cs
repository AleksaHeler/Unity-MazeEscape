using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
	public void PickUp()
	{
		Debug.Log("I am being picked up");
		Destroy(gameObject);
	}
}
