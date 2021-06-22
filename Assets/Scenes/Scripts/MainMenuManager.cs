using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
	// TODO: add main menu animations as in splashscreen

	public void PlayGame()
	{
		SceneLoader.Instance.LoadScene("Game");
	}

	public void QuitGame()
	{
		Application.Quit();
	}
}
