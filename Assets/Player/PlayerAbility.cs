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

    public int Coins { get; private set; }


	private void Awake()
	{
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
	}

	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
            Explode();
        }
    }

    private void Explode()
	{
        Instantiate(explosionParticles, transform.position, Quaternion.Euler(90, 0, 0), transform);
        CameraShake.Instance.ShakeCamera(cameraShakeAmount, cameraShakeDuration);
        MazeMaster.Instance.DestryPlatformsInRange(transform.position, explosionRange);
        AudioManager.Instance.Play("Explosion");
    }
}
