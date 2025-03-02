using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro; // Si tu utilises TextMeshPro

public class FonduTransition : MonoBehaviour
{
    public Image imageNoire;
    public TextMeshProUGUI messageMissionTerminee; // R�f�rence au texte affich�
    public float dureeFondu = 1f; //Le fondu durera une seconde
    public float delaiAvantFondu = 2.0f; // Temps d'affichage du message

    private void Start()
    {
        // D�marrer la sc�ne avec un fondu entrant
        StartCoroutine(FonduEntrant());
    }

    public void AfficherMessageEtChargerScene(string nomScene)
    {
        StartCoroutine(SequenceFinMission(nomScene));
    }

    //Coroutine pour g�rer la fin de la mission et partir le fondu noir
    IEnumerator SequenceFinMission(string nomScene)
    {
        // Afficher le message de fin de mission
        if (messageMissionTerminee != null) //Au besoin et � d�finir dans l'inspecteur
        {
            messageMissionTerminee.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(delaiAvantFondu);

        StartCoroutine(FonduSortant(nomScene)); //Fait le fondu noir vers la prochaine sc�ne
    }

    //Coroutine pour g�rer le fondu en jouant avec les param�tres d'opacit� ou d'alpha
    IEnumerator FonduEntrant()
    {
        float t = dureeFondu;
        while (t > 0)
        {
            t -= Time.deltaTime;
            imageNoire.color = new Color(0, 0, 0, t / dureeFondu); //Diminue l'opacit� progressivement
            yield return null;
        }
        imageNoire.gameObject.SetActive(false); //D�sactive compl�tement l'image noire lorsque le fondu est termin� afin d'afficher la sc�ne
    }

    IEnumerator FonduSortant(string nomScene)
    {
        imageNoire.gameObject.SetActive(true);
        float t = 0;
        while (t < dureeFondu)
        {
            t += Time.deltaTime;
            imageNoire.color = new Color(0, 0, 0, t / dureeFondu); //Augmente l'opacit� progressivement
            yield return null;
        }
        SceneManager.LoadScene(nomScene);
    }
}
