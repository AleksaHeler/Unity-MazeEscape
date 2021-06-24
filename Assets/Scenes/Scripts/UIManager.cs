using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	[SerializeField]
	private Animator sceneTransitionAnimator;
	[SerializeField]
	private float sceneTransitionDuration = 1f;
	[SerializeField]
	private string gameMusicName = "Game Music";

	private void Start()
    {
        sceneTransitionAnimator.Play("FadeIn");
		AudioManager.Instance.FadeIn(gameMusicName, sceneTransitionDuration / 2f);

		PlayerAbility.OnPlayerDeath += OnPlayerDeath;
		PlayerAbility.OnPortalEnter += OnPortalEnter;
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
