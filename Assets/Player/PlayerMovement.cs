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
    [SerializeField]
    private float rotationSpeed = 20f;

    private Rigidbody2D rigidbody;
    private Vector3 movement;

    private void Start()
	{
        rigidbody = GetComponent<Rigidbody2D>();
    }

	void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 direction = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);

    }

	private void FixedUpdate()
	{
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10;
            Vector3 direction = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
            MoveForward(direction);
        }
    }

	private void MoveForward(Vector3 direction)
    {
        movement = direction.normalized;
        rigidbody.AddForce(movement * acceleration);
        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, speed);
	}
}
