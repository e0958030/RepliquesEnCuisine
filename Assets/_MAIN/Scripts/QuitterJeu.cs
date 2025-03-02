using UnityEngine;
using UnityEngine.UI; // Nécessaire si vous manipulez des composants UI

public class QuitterJeu : MonoBehaviour
{
    //Script pour quitter l'application lorsque le jeu est terminé.

    //Bouton qui gère la fin du jeu
    public Button quitButton;

    void Start()
    {
        // Vérifie que le bouton est bien assigné
        if (quitButton != null)
        {
            // Ajoute un listener au clic du bouton
            quitButton.onClick.AddListener(Quit);
        }
    }

    void Update()
    {
        //Le joueur peut aussi quitter le jeu en appuyant sur la touche d'échappement 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Arrête le mode Play dans l'éditeur
#else
            Application.Quit(); // Quitte l'application dans une build
#endif
    }
}
