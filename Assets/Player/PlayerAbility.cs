using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    public static PlayerAbility Instance { get; private set; }

    [SerializeField]
    private float cameraShakeAmount = 3f;
    [SerializeField]
    private float cameraShakeDuration = 0.1f;
    [SerializeField]
    private float explosionRange = 2f;
    [SerializeField]
    private ParticleSystem explosionParticles;
    [SerializeField]
    private float dashAcceleration = 40f;

    public int Coins { get; private set; }


    Dictionary<KeyCode, Action> inputHandler;

	private void Awake()
	{
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        inputHandler = new Dictionary<KeyCode, Action>();

        inputHandler.Add(KeyCode.Space, Explode);
        inputHandler.Add(KeyCode.LeftShift, Dash);
    }

	void Update()
    {
        // Invoke functions on specific key presses
        foreach(KeyCode keyCode in inputHandler.Keys)
		{
            if (Input.GetKeyDown(keyCode))
                inputHandler[keyCode]();
		}
    }

    private void Explode()
	{
        Instantiate(explosionParticles, transform.position, Quaternion.Euler(90, 0, 0), transform);
        CameraShake.Instance.ShakeCamera(cameraShakeAmount, cameraShakeDuration);
        MazeMaster.Instance.DestroyPlatformsInRange(transform.position, explosionRange);
        AudioManager.Instance.Play("Explosion");
    }

    private void Dash()
    {
        AudioManager.Instance.Play("Dash");
        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");
        Vector2 velocity = new Vector3(inputX, inputY);
        if(velocity.magnitude == 0)
            velocity = GetComponent<Rigidbody2D>().velocity;
        velocity.Normalize();
        Vector2 force = velocity * dashAcceleration * (1f / Time.fixedDeltaTime);
        GetComponent<Rigidbody2D>().AddForce(force);
	}
}
