using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalFragment : MonoBehaviour, IItem
{
	public void PickUp()
	{
		PlayerAbility.Instance.AddPortalFragment();
		AudioManager.Instance.Play("Pop");
		Destroy(gameObject);
	}
}
