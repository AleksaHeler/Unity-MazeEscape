using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour, IAudioManager
{
	public static AudioManager Instance { get; private set; }

	[SerializeField]
	private List<Sound> sounds;

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

		foreach (Sound sound in sounds)
		{
			sound.source = gameObject.AddComponent<AudioSource>();

			sound.source.clip = sound.clip;
			sound.source.volume = sound.volume;
			sound.source.pitch = sound.pitch;
			sound.source.loop = sound.loop;
		}
	}

	public void Play(string name)
	{
		foreach(Sound sound in sounds)
		{
			if(sound.name == name)
			{
				sound.source.Play();
				return;
			}
		}
	}

	public void Pause(string name)
	{
		throw new System.NotImplementedException();
	}

	public void Stop(string name)
	{
		throw new System.NotImplementedException();
	}

	public void FadeIn(string name, float duration)
	{
		throw new System.NotImplementedException();
	}

	public void FadeOut(string name, float duration)
	{
		throw new System.NotImplementedException();
	}
}
