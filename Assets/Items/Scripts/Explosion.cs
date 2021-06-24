using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IItem
{
	public void PickUp()
	{
		PlayerAbility.Instance.AddExplosion();
		Destroy(gameObject);
	}
}
