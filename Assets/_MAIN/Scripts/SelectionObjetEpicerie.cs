using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script pour la sélection des objets de la liste d'épicerie
public class SelectionObjetEpicerie : MonoBehaviour
{
    // Liste des tags des éléments de la liste d'épicerie
    // D'autres éléments seront rajoutés par la suite
    public List<string> tagsObjets = new List<string> { "Oeufs", "Creme", "Epinards", "SelPoivre" }; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (tagsObjets.Contains(hit.collider.tag)) // Vérifie si le tag est dans la liste
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
