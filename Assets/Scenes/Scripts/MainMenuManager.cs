using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
	[SerializeField]
	private Transform playerAnimationTransform;
	[SerializeField]
	private Animator transitionAnimator;
	[SerializeField]
	private Animator playButtonAnimator;
	[SerializeField]
	private Animator quitButtonAnimator;
	[SerializeField]
	private float transitionAnimationDuration = 1f;
	[SerializeField]
	private string mainMenuMusicName = "Main Menu Music";

	private void Start()
	{
		transitionAnimator.Play("FadeIn");
		playButtonAnimator.Play("FadeIn");
		quitButtonAnimator.Play("FadeIn");
		AudioManager.Instance.FadeIn(mainMenuMusicName, transitionAnimationDuration / 2f);
	}

	private void Update()
	{
		// Animating player character in main menu
		float time = Time.timeSinceLevelLoad * 0.2f;

		float x = 2 * Mathf.PerlinNoise(time * 2f, time * 0.5f) - 1;
		float y = 2 * Mathf.PerlinNoise(time * 0.5f, time * 2f) - 1;
		x = Mathf.Clamp(x * 8f, -8.2f, 8.2f);
		y = Mathf.Clamp(y * 6f, -4.4f, 4.4f);
		playerAnimationTransform.position = new Vector3(x, y);
	}

	public void PlayGame()
	{
		StartCoroutine(PlayGameCoroutine());
	}

	public void QuitGame()
	{
		StartCoroutine(QuitGameCoroutine());
	}

	IEnumerator PlayGameCoroutine()
	{
		transitionAnimator.Play("FadeOut");
		playButtonAnimator.Play("FadeOut");
		quitButtonAnimator.Play("FadeOut");
		AudioManager.Instance.FadeOut(mainMenuMusicName, transitionAnimationDuration / 2f);
		yield return new WaitForSeconds(transitionAnimationDuration);
		SceneLoader.Instance.LoadScene("Game");
	}

	IEnumerator QuitGameCoroutine()
	{
		transitionAnimator.Play("FadeOut");
		playButtonAnimator.Play("FadeOut");
		quitButtonAnimator.Play("FadeOut");
		AudioManager.Instance.FadeOut(mainMenuMusicName, transitionAnimationDuration / 2f);
		yield return new WaitForSeconds(transitionAnimationDuration);
		Application.Quit();
	}
}
