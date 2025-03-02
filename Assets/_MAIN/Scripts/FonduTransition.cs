using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Si tu utilises TextMeshPro

public class FonduTransition : MonoBehaviour
{
    public Image imageNoire;
    public TextMeshProUGUI messageMissionTerminee; // Référence au texte affiché
    public float dureeFondu = 1f; //Le fondu durera une seconde
    public float delaiAvantFondu = 2.0f; // Temps d'affichage du message

    private void Start()
    {
        // Démarrer la scène avec un fondu entrant
        StartCoroutine(FonduEntrant());
    }

    public void AfficherMessageEtChargerScene(string nomScene)
    {
        StartCoroutine(SequenceFinMission(nomScene));
    }

    IEnumerator SequenceFinMission(string nomScene)
    {
        // Afficher le message de fin de mission
        if (messageMissionTerminee != null)
        {
            messageMissionTerminee.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(delaiAvantFondu);

        StartCoroutine(FonduSortant(nomScene));
    }

    //
    IEnumerator FonduEntrant()
    {
        float t = dureeFondu;
        while (t > 0)
        {
            t -= Time.deltaTime;
            imageNoire.color = new Color(0, 0, 0, t / dureeFondu);
            yield return null;
        }
        imageNoire.gameObject.SetActive(false);
    }

    IEnumerator FonduSortant(string nomScene)
    {
        imageNoire.gameObject.SetActive(true);
        float t = 0;
        while (t < dureeFondu)
        {
            t += Time.deltaTime;
            imageNoire.color = new Color(0, 0, 0, t / dureeFondu);
            yield return null;
        }
        SceneManager.LoadScene(nomScene);
    }
}
