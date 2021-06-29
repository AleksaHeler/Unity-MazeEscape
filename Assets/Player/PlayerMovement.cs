using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float speed = 2f;
    [SerializeField]
    private float acceleration = 450f;
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

	private void OnDestroy()
    {
        PlayerAbility.OnPortalEnter -= SnapToPortal;
    }

	void Update()
    {
        // Look towards the mouse
        Vector3 target = GetMousePositionInWorldCoordinates();
        Vector3 direction = target - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
    }

	private void FixedUpdate()
    {
        // Move the player
        float distanceFromPlayerToMouse = Vector3.Distance(transform.position, GetMousePositionInWorldCoordinates());
        if (freeMovement && Input.GetMouseButton(0) && distanceFromPlayerToMouse > 0.5f)
        {
            Vector3 moveDirection = GetMousePositionInWorldCoordinates() - transform.position;
            MoveForward(moveDirection);
        }

        if (!freeMovement)
        {
            Vector3 moveDirection = snappedPortalPosition - transform.position;
            MoveForward(moveDirection);
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
        Vector3 movement = Vector3.ClampMagnitude(direction, 2f) * 0.5f;
		rigidbody.AddForce(movement * acceleration * Time.fixedDeltaTime);
        Vector2 velocityBefore = rigidbody.velocity;
        rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, speed);
    }

    public void SnapToPortal(Vector3 portalPosition)
	{
        freeMovement = false;
        snappedPortalPosition = portalPosition;
	}
}
