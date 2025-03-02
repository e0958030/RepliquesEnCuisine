using UnityEngine;
using UnityEngine.UI; // N�cessaire si vous manipulez des composants UI

public class QuitterJeu : MonoBehaviour
{
    //Script pour quitter l'application lorsque le jeu est termin�.

    //Bouton qui g�re la fin du jeu
    public Button quitButton;

    void Start()
    {
        // V�rifie que le bouton est bien assign�
        if (quitButton != null)
        {
            // Ajoute un listener au clic du bouton
            quitButton.onClick.AddListener(Quit);
        }
    }

    void Update()
    {
        //Le joueur peut aussi quitter le jeu en appuyant sur la touche d'�chappement 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Arr�te le mode Play dans l'�diteur
#else
            Application.Quit(); // Quitte l'application dans une build
#endif
    }
}
