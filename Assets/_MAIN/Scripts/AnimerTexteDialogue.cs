using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Ce script provient du tutoriel de Pixelbug Studio sur YouTube (Type Writing effect in Unity using TextMeshPro)
//https://www.youtube.com/watch?v=IqpgJlhtmoo
public class AnimerTexteDialogue : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _textMeshPro;

    public string[] stringArray;

    [SerializeField] float tempsEntreLettres;
    [SerializeField] float tempsEntreMots;

    int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        FinEcriture();
    }

    public void FinEcriture()
    {
        if (i <= stringArray.Length - 1)
        {
            _textMeshPro.text = stringArray[i];
            StartCoroutine(TexteVisible());
        }
    }


    private IEnumerator TexteVisible()
    {
        _textMeshPro.ForceMeshUpdate();
        int totalLettresVisibles = _textMeshPro.textInfo.characterCount;
        int compteurLettres = 0;

        while (true)
        {
            int nbLettresVisibles = compteurLettres % (totalLettresVisibles + 1);
            _textMeshPro.maxVisibleCharacters = nbLettresVisibles;

            if (nbLettresVisibles >= totalLettresVisibles)
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
