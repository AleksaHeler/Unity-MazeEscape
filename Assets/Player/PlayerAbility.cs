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
    [SerializeField]
    private GameObject portalPrefab;
    [Header("Dash settings")]
    [SerializeField]
    private float dashDistance = 2f;
    [SerializeField]
    private float dashCooldown = 2f;
    private float dashTimer = 0f;
    private GameObject snappedPortal;

    public int ExplosionsCount { get; private set; }
    public int CoinsCount { get; private set; }
    public int PortalFragments { get; private set; }
    public float DashTimer { get => dashTimer; }
    public float DashCooldownTime { get => dashCooldown; }
    public float ExplosionRange { get => explosionRange; }


    Dictionary<KeyCode, Action> inputHandler;
    public static event Action OnPlayerDeath = delegate { };
    public static event Action<Vector3> OnPortalEnter = delegate { };
    public static event Action OnLevelChanged = delegate { };
    public static event Action<Vector3> OnExplosion = delegate { };

    private void Awake()
	{
        Instance = this;

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

        // Check if we have all fragments to create the portal
        HandlePortal();
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
        OnExplosion(transform.position);
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
        if (collision.gameObject.tag.Equals("Enemy"))
            Die();
    }

    private void HandleItemCollection(GameObject itemGameObject)
	{
        IItem item = itemGameObject.GetComponent<IItem>();
        if (item != null)
            item.PickUp();
    }

    private void HandlePortal()
	{
        if(PortalFragments < 3)
		{
            return;
		}

        PortalFragments = 0;
        ExplosionsCount = startingExplosions;
        dashTimer = 0;
        Vector3Int position = new Vector3Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), 0);
        snappedPortal = Instantiate(portalPrefab, position, Quaternion.identity);
        Time.timeScale = 0.6f;
        OnPortalEnter(snappedPortal.transform.position);
    }

    public void TriggerLevelChange()
    {
        Time.timeScale = 1f;
        Destroy(snappedPortal);
        OnLevelChanged();
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
    public void AddPortalFragment()
    {
        PortalFragments++;
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

    private void Die()
	{
        OnPlayerDeath();
        Destroy(gameObject);
    }
}
