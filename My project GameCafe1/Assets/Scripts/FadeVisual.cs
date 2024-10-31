using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeVisual : MonoBehaviour
{
    [SerializeField] private Image fadeImage;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float pauseTime = 3;
    private bool isPlaying = false;

    private void OnEnable()
    {
        FadeEvent.OnFade += PlayEffect;
    }

    private void OnDisable()
    {
        FadeEvent.OnFade -= PlayEffect;
    }

    public void PlayEffect()
    {
        if (isPlaying) return;
        StartCoroutine(FadeEffectCoroutine());
    }

    private IEnumerator FadeEffectCoroutine()
    {
        isPlaying = true;
        fadeImage.gameObject.SetActive(true);

        yield return FadeIn();
        yield return new WaitForSeconds(pauseTime); 
        yield return FadeOut(); 

        fadeImage.gameObject.SetActive(false);
        isPlaying = false;
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 1;
        fadeImage.color = color;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            color.a = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            fadeImage.color = color;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        color.a = 0;
        fadeImage.color = color;
    }
}

