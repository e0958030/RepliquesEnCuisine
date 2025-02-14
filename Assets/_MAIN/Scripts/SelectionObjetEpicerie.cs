using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script pour la s�lection des objets de la liste d'�picerie
public class SelectionObjetEpicerie : MonoBehaviour
{
    // Liste des tags des �l�ments de la liste d'�picerie
    // D'autres �l�ments seront rajout�s par la suite
    public List<string> tagsObjets = new List<string> { "Oeufs", "Creme", "Epinards", "SelPoivre" }; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (tagsObjets.Contains(hit.collider.tag)) // V�rifie si le tag est dans la liste
                {
                    Destroy(hit.collider.gameObject);
                }
            }
        }
    }
}
