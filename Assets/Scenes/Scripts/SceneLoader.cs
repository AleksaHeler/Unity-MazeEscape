using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
	public static SceneLoader Instance { get; private set; }

	[SerializeField]
	private List<string> sceneNames;

	private IAudioManager audioManager;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	public void LoadScene(int index)
	{
		LoadScene(sceneNames[index]);
	}

	public void LoadScene(string name)
	{
		SceneManager.LoadScene(name);
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{

	}
}
