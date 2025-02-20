using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    void Update()
    {
        //Quitter le jeu en appuyant sur la touche échapper
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Arrête le mode Play dans l'éditeur
        #else
            Application.Quit(); // Quitte l'application dans une build
        #endif
    }
}
