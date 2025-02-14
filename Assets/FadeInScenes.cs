using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInScenes : MonoBehaviour
{
    //Référence au canvasGroup
    [SerializeField] private CanvasGroup canvasGroup;

    //Durée du fade in
    [SerializeField] private float dureeFade = 2f;

    [SerializeField] private bool fadeIn = false;

    private void Start()
    {
        if (fadeIn)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }

    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0, dureeFade));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, dureeFade));
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float start, float end, float duration)
    {
        float tempsEcoule = 0.0f;
        while (tempsEcoule < dureeFade)
        {
            tempsEcoule += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, tempsEcoule / duration);
            yield return null;
        }

        cg.alpha = end;
    }
}
