using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SauterScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Passer la scène en appuyant sur la touche d'espacement
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("NiveauIntro_Gare");
        }
    }
}
