using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SauterScene : MonoBehaviour
{
    public string nomScene; // Nom de la scène à charger
    public Image fonduNoir; // Image noire pour le fondu
    public float dureeFondu = 1f; // Durée du fondu

    private void Start()
    {
        //L'image de fondu est activée et complètement opaque
        fonduNoir.gameObject.SetActive(true);
        fonduNoir.color = new Color(0, 0, 0, 1);

        //Lancer le fondu d'entrée
        StartCoroutine(FonduEntree());
    }

    //Lorsque le joueur appuie sur la touche d'espacement, la scène suivante se charge avec un effet de fondu noir progressif géré dans la coroutine plus bas
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(FaireFonduEtChangerScene());
        }
    }

    IEnumerator FonduEntree()
    {
        float elapsedTime = 0f;

        while (elapsedTime < dureeFondu)
        {
            elapsedTime += Time.deltaTime;
            float alpha = 1 - Mathf.Clamp01(elapsedTime / dureeFondu);
            fonduNoir.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        fonduNoir.gameObject.SetActive(false); // Désactiver l'image après le fondu
    }

    IEnumerator FaireFonduEtChangerScene()
    {
        fonduNoir.gameObject.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < dureeFondu)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / dureeFondu);
            fonduNoir.color = new Color(0, 0, 0, alpha);
            yield return null;
        }

        SceneManager.LoadScene(nomScene);
    }
}
