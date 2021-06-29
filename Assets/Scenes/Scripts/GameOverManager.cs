using System.Collections;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
	[SerializeField]
	private Animator transitionAnimator;
	[SerializeField]
	private float transitionAnimationDuration = 1f;
	[SerializeField]
	private string mainMenuMusicName = "Main Menu Music";
	[SerializeField]
	private string mainMenuSceneName = "MainMenu";

	private void Start()
	{
		transitionAnimator.Play("FadeIn");
		AudioManager.Instance.FadeIn(mainMenuMusicName, transitionAnimationDuration / 2f);
	}

	public void MainMenu()
	{
		StartCoroutine(MainMenuCoroutine());
	}

	public void QuitGame()
	{
		StartCoroutine(QuitGameCoroutine());
	}


	IEnumerator QuitGameCoroutine()
	{
		transitionAnimator.Play("FadeOut");
		AudioManager.Instance.FadeOut(mainMenuMusicName, transitionAnimationDuration / 2f);
		yield return new WaitForSeconds(transitionAnimationDuration);
		Application.Quit();
	}

	IEnumerator MainMenuCoroutine()
	{
		transitionAnimator.Play("FadeOut");
		AudioManager.Instance.FadeOut(mainMenuMusicName, transitionAnimationDuration / 2f);
		yield return new WaitForSeconds(transitionAnimationDuration);
		SceneLoader.Instance.LoadScene(mainMenuSceneName);
	}
}
