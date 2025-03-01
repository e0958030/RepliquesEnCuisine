using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;

public class ChangerSceneBouton : MonoBehaviour, IPointerClickHandler
{
    ////////// Déclaration des variables //////////

    //Variable pour mettre le nom de la scène à changer
    public string sceneACharger;

    // Musique et effets sonores
    private AudioSource audioSource;
    public AudioClip sonClicBouton; // Pour la rétroaction sur le clic de souris
    private float delaiAvantChangement = 2f; // Délai de 2 secondes

    // Start est appelé avant la première frame update
    void Start()
    {
        // Récupérer l'AudioSource attachée à l'objet
        audioSource = GetComponent<AudioSource>();
    }

    // Update est appelé à chaque frame
    void Update()
    {
        // Si le joueur appuie sur la touche « Entrée », jouer le son puis changer de scène
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

        // Attendre 2 secondes avant de changer de scène
        Invoke("ChangeScene", delaiAvantChangement);
    }

    // Script pour le changement de scène sur un clic de bouton
    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneACharger);
    }
}