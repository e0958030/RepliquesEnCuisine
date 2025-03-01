using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class ChangerSceneBouton : MonoBehaviour, IPointerClickHandler
{
    ////////// D�claration des variables //////////

    //Variable pour mettre le nom de la sc�ne � changer
    public string sceneACharger;

    // Musique et effets sonores
    private AudioSource audioSource;
    public AudioClip sonClicBouton; // Pour la r�troaction sur le clic de souris
    private float delaiAvantChangement = 2f; // D�lai de 2 secondes

    // Start est appel� avant la premi�re frame update
    void Start()
    {
        // R�cup�rer l'AudioSource attach�e � l'objet
        audioSource = GetComponent<AudioSource>();
    }

    // Update est appel� � chaque frame
    void Update()
    {
        // Si le joueur appuie sur la touche � Entr�e �, jouer le son puis changer de sc�ne
        if (Input.GetKeyDown(KeyCode.Return))
        {
            JouerSonEtChangerScene();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        JouerSonEtChangerScene();
    }

    void JouerSonEtChangerScene()
    {
        if (audioSource != null && sonClicBouton != null)
        {
            // Jouer le son du clic
            audioSource.PlayOneShot(sonClicBouton);
        }

        // Attendre 2 secondes avant de changer de sc�ne
        Invoke("ChangeScene", delaiAvantChangement);
    }

    // Script pour le changement de sc�ne sur un clic de bouton
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneACharger);
    }
}