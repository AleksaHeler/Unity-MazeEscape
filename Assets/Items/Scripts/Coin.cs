using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour, IItem
{
	public void PickUp()
	{
		PlayerAbility.Instance.AddCoin();
		AudioManager.Instance.Play("Pop");
		Destroy(gameObject);
	}
}
