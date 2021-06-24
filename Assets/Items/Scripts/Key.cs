using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IItem
{
	public void PickUp()
	{
		PlayerAbility.Instance.AddKey();
		Destroy(gameObject);
	}
}
