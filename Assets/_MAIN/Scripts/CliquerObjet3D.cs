using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CliquerObjet3D : MonoBehaviour
{
    //Ajouter un effet de hover sur les objets pour rétroaction 
    private void OnMouseEnter()
    {
        AugmenterTaille(true);
    }

    private void OnMouseExit()
    {
        AugmenterTaille(false);
    }

    private Vector3 tailleInitiale;
    
    private void Awake()
    {
        tailleInitiale = transform.localScale;
    }

    //Augmenter la taille
    private void AugmenterTaille(bool status)
    {
        Vector3 tailleFinale = tailleInitiale;

        //Si la condition est vraie, la taille augmente
        if(status)
            tailleFinale = tailleInitiale * 1.1f;
            transform.localScale = tailleFinale;

    // Update is called once per frame
    // void Update()
    // {
    //     Sur clic de la souris sur l'objet 3D, il activera la scène correspondante.
    //     if(Input.GetMouseButtonDown(0)){
    //         Ray ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
    //         RaycastHit hit;

    //         if (Physics.Raycast(ray, out hit)){
    //             Debug.Log("L'objet a été cliqué");
    //         }
    //     }
    // }
    }
}
