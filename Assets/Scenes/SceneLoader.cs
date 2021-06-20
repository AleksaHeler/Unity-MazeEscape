using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{
	private static SceneLoader instance;
	public static SceneLoader Instance { get => instance; }

	[SerializeField]
	private List<string> sceneNames;

	private IAudioManager audioManager;


	private void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			instance = this;
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
		throw new System.NotImplementedException();
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{

	}
}
