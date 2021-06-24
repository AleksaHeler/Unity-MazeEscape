using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class Tail : MonoBehaviour
{
	[Header("Basic tail settings")]
	[SerializeField]
	private Transform targetDirection;
	[SerializeField]
	private int length = 30;
	[SerializeField]
	private float targetDistance = 0.1f;

	[Header("Animation settings")]
	[SerializeField]
	private float smoothSpeed = 0.1f;
	[SerializeField]
	private float trailSpeed = 350f;

	[Header("Wiggle settings")]
	[SerializeField]
	private float wiggleSpeed = 10f;
	[SerializeField]
	private float wiggleMagnitude = 20f;
	[SerializeField]
	private Transform wiggleDirection;

	private LineRenderer lineRenderer;
	private Vector3[] segmentPoses;
	private Vector3[] segmentVertices;
		
	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
		lineRenderer.positionCount = length;
		segmentPoses = new Vector3[length];
		segmentVertices = new Vector3[length];
	}

	private void Update()
	{
		// Wiggle
		wiggleDirection.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time * wiggleSpeed) * wiggleMagnitude);

		// Set first point to be at the head
		segmentPoses[0] = targetDirection.position;

		// Animate all points
		for (int i = 1; i < length; i++)
		{
			segmentPoses[i] = Vector3.SmoothDamp(segmentPoses[i], segmentPoses[i - 1] + targetDirection.right * targetDistance, ref segmentVertices[i], smoothSpeed + i / trailSpeed);
		}

		lineRenderer.SetPositions(segmentPoses);
	}
}
