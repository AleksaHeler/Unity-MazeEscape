using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	[SerializeField]
	private float speed = 1f;
	private Transform target;
	private Vector3 offset;

	private void Awake()
	{
		target = GameObject.FindGameObjectWithTag("Player").transform;
		offset = transform.position - target.position;
	}

	private void LateUpdate()
	{
		Vector3 destination = target.position + offset;
		Vector3 movement = (destination - transform.position) * speed * Time.deltaTime;
		transform.position += movement;
	}
}
