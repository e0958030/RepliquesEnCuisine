using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SauterScene : MonoBehaviour
{
    //Simple script pour changer de sc�ne en appuyant la touche espace
    public string nomScene;
    
    // Update is called once per frame
    void Update()
    {
        //Passer la sc�ne en appuyant sur la touche d'espacement
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nomScene);
        }
    }
}
