using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float acceleration = 15f;

    private Rigidbody2D rigidbody;
    private Vector3 movement;

    private void Start()
	{
        rigidbody = GetComponent<Rigidbody2D>();
    }

	void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        movement = new Vector3(inputX, inputY);
    }

    void FixedUpdate()
	{
        rigidbody.AddForce(movement * acceleration);
        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, speed);
	}
}
