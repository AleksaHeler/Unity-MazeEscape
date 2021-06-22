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
		foreach (Sound sound in sounds)
		{
			if (sound.name == name)
			{
				sound.source.Pause();
				return;
			}
		}
	}

	public void Stop(string name)
	{
		foreach (Sound sound in sounds)
		{
			if (sound.name == name)
			{
				sound.source.Stop();
				return;
			}
		}
	}

	public void FadeIn(string name, float duration)
	{
		foreach (Sound sound in sounds)
		{
			if (sound.name == name)
			{
				StartCoroutine(FadeInCoroutine(sound, duration));
				return;
			}
		}
	}

	public void FadeOut(string name, float duration)
	{
		foreach (Sound sound in sounds)
		{
			if (sound.name == name)
			{
				StartCoroutine(FadeOutCoroutine(sound, duration));
				return;
			}
		}
	}

	IEnumerator FadeInCoroutine(Sound sound, float duration)
	{
		float originalVolume = sound.volume;
		
		sound.source.volume = 0;
		sound.source.Play();

		float elapsedTime = 0;
		while(elapsedTime < duration)
		{
			float percent = elapsedTime / duration;
			sound.source.volume = Mathf.Lerp(0f, originalVolume, percent);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		sound.source.volume = originalVolume;
	}
	IEnumerator FadeOutCoroutine(Sound sound, float duration)
	{
		float originalVolume = sound.volume;

		sound.source.volume = originalVolume;

		float elapsedTime = 0;
		while (elapsedTime < duration)
		{
			float inversePercent = 1f - elapsedTime / duration;
			sound.source.volume = Mathf.Lerp(0f, originalVolume, inversePercent);
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		sound.source.Stop();
		sound.source.volume = originalVolume;
	}
}
