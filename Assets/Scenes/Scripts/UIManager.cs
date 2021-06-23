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
}
