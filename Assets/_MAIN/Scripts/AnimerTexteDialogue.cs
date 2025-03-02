using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Ce script provient du tutoriel de Pixelbug Studio sur YouTube (Type Writing effect in Unity using TextMeshPro)
//https://www.youtube.com/watch?v=IqpgJlhtmoo
public class AnimerTexteDialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro; //R�f�rence au TextMeshPro qui affiche le dialogue

    public string[] stringArray; //Tableau contenant toutes les phrases � afficher (� d�finir dans l'inspecteur)

    [SerializeField] float tempsEntreLettres;
    [SerializeField] float tempsEntreMots;

    int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        FinEcriture();
    }

    //Affiche la phrase courante avec son animation d'�criture
    public void FinEcriture()
    {
        if (i <= stringArray.Length - 1)
        {
            _textMeshPro.text = stringArray[i];
            StartCoroutine(TexteVisible());
        }
    }

    //Coroutine qui affiche progressivement les lettres de la phrase selon les d�lais pr�cis�s dans l'inspecteur
    private IEnumerator TexteVisible()
    {
        _textMeshPro.ForceMeshUpdate(); //Force une mise � jour du texte pour l'effet d'animation
        int totalLettresVisibles = _textMeshPro.textInfo.characterCount;
        int compteurLettres = 0;

        while (true)
        {
            int nbLettresVisibles = compteurLettres % (totalLettresVisibles + 1);
            _textMeshPro.maxVisibleCharacters = nbLettresVisibles;

            if (nbLettresVisibles >= totalLettresVisibles) //Passera � la phrase suivante une fois que tout le texte sera affich�
            {
                i += 1;
                Invoke("FinEcriture", tempsEntreMots);
                break;
            }

            compteurLettres++;
            yield return new WaitForSeconds(tempsEntreLettres);
        }
    }
}
