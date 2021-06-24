using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbility : MonoBehaviour
{
    private const string playerPrefsCoinsKey = "Coins";
    public static PlayerAbility Instance { get; private set; }

    [Header("Explosion settings")]
    [SerializeField]
    private float cameraShakeAmount = 3f;
    [SerializeField]
    private float cameraShakeDuration = 0.1f;
    [SerializeField]
    private float explosionRange = 2f;
    [SerializeField]
    private int startingExplosions = 2;
    [SerializeField]
    private int maxExplosions = 5;
    [SerializeField]
    private ParticleSystem explosionParticles;
    [Header("Dash settings")]
    [SerializeField]
    private float dashDistance = 2f;
    [SerializeField]
    private float dashCooldown = 2f;
    private float dashTimer = 0f;

    public int ExplosionsCount { get; private set; }
    public int CoinsCount { get; private set; }
    public int KeysCount { get; private set; }
    public float DashTimer { get => dashTimer; }
    public float DashCooldownTime { get => dashCooldown; }


    Dictionary<KeyCode, Action> inputHandler;
    public static event Action OnPlayerDeath = delegate { };
    public static event Action OnPortalEnter = delegate { };

    private void Awake()
	{
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        inputHandler = new Dictionary<KeyCode, Action>();

        inputHandler.Add(KeyCode.Space, Explode);
        inputHandler.Add(KeyCode.Mouse1, Dash);

        ExplosionsCount = startingExplosions;
        LoadCoinsFromPrefs();
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
        if(ExplosionsCount <= 0)
		{
            return;
		}
        Instantiate(explosionParticles, transform.position, Quaternion.Euler(90, 0, 0), transform);
        CameraShake.Instance.ShakeCamera(cameraShakeAmount, cameraShakeDuration);
        MazeMaster.Instance.DestroyPlatformsInRange(transform.position, explosionRange);
        AudioManager.Instance.Play("Explosion");
        ExplosionsCount--;
    }

    private void Dash()
    {
        if (dashTimer > 0f)
            return;
        dashTimer = dashCooldown;
        AudioManager.Instance.Play("Dash");
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10;
        Vector3 velocity = Camera.main.ScreenToWorldPoint(mousePos) - transform.position;
        if(velocity.magnitude == 0)
            velocity = GetComponent<Rigidbody2D>().velocity;
        velocity.Normalize();
        Vector3 force = velocity * dashDistance;
        GetComponent<Rigidbody2D>().MovePosition(transform.position + force);
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
	{
        while(dashTimer > 0)
		{
            dashTimer -= Time.deltaTime;
            yield return null;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only collect items with right tags
        if (collision.gameObject.tag.Equals("Item"))
            HandleItemCollection(collision.gameObject);

        if (collision.gameObject.tag.Equals("Portal"))
            HandlePortal();

    }

    private void HandleItemCollection(GameObject itemGameObject)
	{
        IItem item = itemGameObject.GetComponent<IItem>();
        if (item != null)
            item.PickUp();
    }

    private void HandlePortal()
	{
        if(KeysCount < 3)
		{
            return;
		}

        Debug.Log("Traveling trough portal");
        KeysCount = 0;
        OnPortalEnter();
    }

    public void AddCoin()
    {
        CoinsCount++;
        PlayerPrefs.SetInt(playerPrefsCoinsKey, CoinsCount);
    }
    public void AddExplosion()
    {
        ExplosionsCount++;
    }
    public void AddKey()
    {
        KeysCount++;
    }

    private void LoadCoinsFromPrefs()
	{
		if (PlayerPrefs.HasKey(playerPrefsCoinsKey))
        {
            CoinsCount = PlayerPrefs.GetInt(playerPrefsCoinsKey);
        }
		else
        {
            CoinsCount = 0;
            PlayerPrefs.SetInt(playerPrefsCoinsKey, 0);
        }
	}
}
