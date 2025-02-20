using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    void Update()
    {
        //Quitter le jeu en appuyant sur la touche �chapper
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    void Quit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false; // Arr�te le mode Play dans l'�diteur
        #else
            Application.Quit(); // Quitte l'application dans une build
        #endif
    }
}
