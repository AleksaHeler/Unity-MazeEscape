using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[Header("Scene transition")]
	[SerializeField]
	private Animator sceneTransitionAnimator;
	[SerializeField]
	private float sceneTransitionDuration = 1f;
	[SerializeField]
	private string gameMusicName = "Game Music";

	[Header("UI")]
	[SerializeField]
	private TMPro.TextMeshProUGUI coinsText;
	private PlayerAbility playerAbility;
	[SerializeField]
	[Range(0f, 1f)]
	private float disabledAlpha = 64f/256f;
	[SerializeField]
	[Range(0f, 1f)]
	private float enabledAlpha = 1f;
	[SerializeField]
	private List<Image> keysImages;
	[SerializeField]
	private List<Image> explosionsImages;
	[SerializeField]
	private Image dashImage;


	private void Start()
	{
		sceneTransitionAnimator.Play("FadeIn");
		AudioManager.Instance.FadeIn(gameMusicName, sceneTransitionDuration / 2f);

		PlayerAbility.OnPlayerDeath += OnPlayerDeath;
		PlayerAbility.OnPortalEnter += OnPortalEnter;

		playerAbility = PlayerAbility.Instance;
	}

	private void OnDestroy()
	{
		PlayerAbility.OnPlayerDeath -= OnPlayerDeath;
		PlayerAbility.OnPortalEnter -= OnPortalEnter;
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			QuitGame();
		}

		HandleCoinsUI();
		HandleKeysUI();
		HandleExplosionsUI();
		HandleDashUI();
	}

	private void HandleCoinsUI()
	{
		if (playerAbility == null)
		{
			playerAbility = PlayerAbility.Instance;
		}
		else
		{
			coinsText.text = playerAbility.CoinsCount.ToString();
		}
	}

	private void HandleKeysUI()
	{
		// Set all to disabled
		foreach(Image image in keysImages)
			SetImageAlpha(image, disabledAlpha);

		// Set right amount to enabled
		int keysCount = playerAbility.KeysCount;
		if (keysCount > keysImages.Count)
			keysCount = keysImages.Count;
		for (int i = 0; i < keysCount; i++)
		{
			SetImageAlpha(keysImages[i], enabledAlpha);
		}
	}

	private void HandleExplosionsUI()
	{
		// Set all to disabled
		foreach (Image image in explosionsImages)
			SetImageAlpha(image, disabledAlpha);

		// Set right amount to enabled
		int explosionsCount = playerAbility.ExplosionsCount;
		if (explosionsCount > explosionsImages.Count)
			explosionsCount = explosionsImages.Count;
		for (int i = 0; i < explosionsCount; i++)
		{
			SetImageAlpha(explosionsImages[i], enabledAlpha);
		}
	}

	private void HandleDashUI()
	{
		float dashTimer = playerAbility.DashTimer;
		if (dashTimer <= 0)
		{
			SetImageAlpha(dashImage, enabledAlpha);
			dashImage.fillAmount = 1f;
			return;
		}

		SetImageAlpha(dashImage, disabledAlpha);
		float dashCooldownTime = playerAbility.DashCooldownTime;
		dashImage.fillAmount = (dashCooldownTime - dashTimer) / dashCooldownTime;
	}

	private void SetImageAlpha(Image image, float alpha)
	{
		Color newColor = image.color;
		newColor.a = alpha;
		image.color = newColor;
	}

	public void QuitGame()
	{
		StartCoroutine(QuitGameCoroutine());
	}

	IEnumerator QuitGameCoroutine()
	{
		sceneTransitionAnimator.Play("FadeOut");
		AudioManager.Instance.FadeOut(gameMusicName, sceneTransitionDuration / 2f);
		yield return new WaitForSeconds(sceneTransitionDuration);
		Application.Quit();
	}

	IEnumerator GameOverCoroutine()
	{
		sceneTransitionAnimator.Play("FadeOut");
		AudioManager.Instance.FadeOut(gameMusicName, sceneTransitionDuration / 2f);
		yield return new WaitForSeconds(sceneTransitionDuration);
		SceneLoader.Instance.LoadScene("GameOver");
	}

	IEnumerator NextLevelCoroutine()
	{
		sceneTransitionAnimator.Play("FadeOut");
		AudioManager.Instance.FadeOut(gameMusicName, sceneTransitionDuration / 2f);
		yield return new WaitForSeconds(sceneTransitionDuration);
		Debug.Log("TODO: implement switching to next level");
	}

	private void OnPlayerDeath()
	{
		Debug.Log("[UIManager] Player died, switching to game over");
		StartCoroutine(GameOverCoroutine());
	}

	private void OnPortalEnter()
	{
		Debug.Log("[UIManager] Player entered portal, switching to next level");
		StartCoroutine(NextLevelCoroutine());
	}
}
