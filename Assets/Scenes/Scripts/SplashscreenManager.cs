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

	private void Start()
    {
        StartCoroutine(AnimateSplashscreen());
    }

    IEnumerator AnimateSplashscreen()
    {
        titleAnimator.Play("FadeIn");
        transitionAnimator.Play("FadeIn");
        yield return new WaitForSeconds(splashscreenDuration + transitionDuration);
        titleAnimator.Play("FadeOut");
        transitionAnimator.Play("FadeOut");
        yield return new WaitForSeconds(transitionDuration);
        SceneLoader.Instance.LoadScene("MainMenu");
    }
}
