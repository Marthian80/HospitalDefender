using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFade : Singleton<UIFade>
{
    [SerializeField] private Image imageFadeScreen;
    [SerializeField] private float fadeSpeed = 1.5f;

    private IEnumerator fadeRoutine;

    public void FadeToBlack()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(1);
        StartCoroutine(fadeRoutine);
    }

    public void FadeToClear()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(0);
        StartCoroutine(fadeRoutine);
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        while (!Mathf.Approximately(imageFadeScreen.color.a, targetAlpha))
        {
            float alpha = Mathf.MoveTowards(imageFadeScreen.color.a, targetAlpha, fadeSpeed * Time.deltaTime);
            imageFadeScreen.color = new Color(imageFadeScreen.color.r, imageFadeScreen.color.b, imageFadeScreen.color.g, alpha);
            yield return null;
        }
    }
}
