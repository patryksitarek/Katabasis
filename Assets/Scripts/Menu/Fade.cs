using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{

    public void FadeOut(float fadeSpeed)
    {
        StartCoroutine(DoFadeOut(fadeSpeed));
    }

    IEnumerator DoFadeOut(float fadeSpeed)
    {
        TryGetComponent<Image>(out Image img);
        if (img == false) yield return null;

        float newAlpha;
        while (img.color.a < 1)
        {
            newAlpha = img.color.a + (fadeSpeed * Time.deltaTime);

            img.color = new Color(img.color.r, img.color.g, img.color.b, newAlpha);
            if (img.color.a <= 0)
            {
                Time.timeScale = 0;
            }
            yield return null;
        }
        Time.timeScale = 0;
    }

    public void FadeIn(float fadeSpeed)
    {
        StartCoroutine(DoFadeIn(fadeSpeed));
    }

    IEnumerator DoFadeIn(float fadeSpeed)
    {
        Time.timeScale = 1;
        TryGetComponent<Image>(out Image img);
        if (img == false) yield return null;

        float newAlpha;
        while (img.color.a > 0)
        {
            newAlpha = img.color.a - (fadeSpeed * Time.deltaTime);

            img.color = new Color(img.color.r, img.color.g, img.color.b, newAlpha);
            yield return null;
        }
    }
}
