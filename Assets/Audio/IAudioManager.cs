using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IAudioManager
{
	void Play(string name);
	void Pause(string name);
	void Stop(string name);
	void FadeIn(string name, float duration);
	void FadeOut(string name, float duration);
}