using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class BoutonSuivant : MonoBehaviour

{
    /////////////Déclaration des variables/////////////
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

    //Pour charger la scène de bataille lorsque le bouton suivant est cliqué
    public void ChargerSceneJeu()
    {
        Invoke("DelaiChargementScene", 1f);
    }
    
    //Charger la scène après le délai indiqué pour que le changement de scène ne soit pas trop brusque
    void DelaiChargementScene()
        {
            SceneManager.LoadScene(sceneACharger); 
        }
  
}

