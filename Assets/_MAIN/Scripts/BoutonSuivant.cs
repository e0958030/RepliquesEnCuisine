using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class BoutonSuivant : MonoBehaviour

{
    /////////////D�claration des variables/////////////
    public string sceneACharger;

    //Musique et effets sonores
    private AudioSource audioSource;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    //Pour charger la sc�ne de bataille lorsque le bouton suivant est cliqu�
    public void ChargerSceneJeu()
    {
        Invoke("DelaiChargementScene", 1f);
    }
    
    //Charger la sc�ne apr�s le d�lai indiqu� pour que le changement de sc�ne ne soit pas trop brusque
    void DelaiChargementScene()
        {
            SceneManager.LoadScene(sceneACharger); 
        }
  
}

