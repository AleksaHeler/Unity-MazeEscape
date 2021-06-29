using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour, IItem
{
	public void PickUp()
	{
		PlayerAbility.Instance.AddExplosion();
		AudioManager.Instance.Play("Pop");
		Destroy(gameObject);
	}
}
