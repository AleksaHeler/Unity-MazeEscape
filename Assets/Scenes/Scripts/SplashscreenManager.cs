using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashscreenManager : MonoBehaviour
{
    [SerializeField]
    private Animator titleAnimator;
    [SerializeField]
    private Animator transitionAnimator;
    [SerializeField]
    private float transitionDuration = 1f;
    [SerializeField]
    private float splashscreenDuration = 1f;
    [SerializeField]
    private string introSoundName = "Intro";

	private void Start()
    {
        StartCoroutine(AnimateSplashscreen());
    }

    IEnumerator AnimateSplashscreen()
    {
        AudioManager.Instance.FadeIn(introSoundName, transitionDuration / 2f);
        titleAnimator.Play("FadeIn");
        transitionAnimator.Play("FadeIn");
        yield return new WaitForSeconds(splashscreenDuration + transitionDuration);
        titleAnimator.Play("FadeOut");
        transitionAnimator.Play("FadeOut");
        yield return new WaitForSeconds(transitionDuration);
        AudioManager.Instance.FadeOut(introSoundName, transitionDuration / 2f);
        SceneLoader.Instance.LoadScene("MainMenu");
    }
}
