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

    // Snapping to portal (animation for going trough)
    private bool freeMovement;
    private Vector3 snappedPortalPosition;

    private void Start()
	{
        rigidbody = GetComponent<Rigidbody2D>();

        // Subscribe to event callback
        PlayerAbility.OnPortalEnter += SnapToPortal;

        // Reset variables
        freeMovement = true;
        snappedPortalPosition = Vector3.zero;
    }

	void Update()
    {
        // If we dont have free movement, look towards the portal
        Vector3 target = freeMovement ? GetMousePositionInWorldCoordinates() : snappedPortalPosition;
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

	private void FixedUpdate()
	{
        if (freeMovement && Input.GetMouseButton(0))
        {
            Vector3 direction = GetMousePositionInWorldCoordinates() - transform.position;
            MoveForward(direction);
        }

		if (!freeMovement)
        {
            Vector3 direction = snappedPortalPosition - transform.position;
            MoveForward(direction);
        }
    }

    private Vector3 GetMousePositionInWorldCoordinates()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

	private void MoveForward(Vector3 direction)
    {
        Vector3 movement = direction.normalized;
        rigidbody.AddForce(movement * acceleration);
        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, speed);
	}

    public void SnapToPortal(Vector3 portalPosition)
	{
        freeMovement = false;
        snappedPortalPosition = portalPosition;
	}
}
