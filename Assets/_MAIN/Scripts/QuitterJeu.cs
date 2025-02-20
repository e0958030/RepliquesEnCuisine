using UnityEngine;
using UnityEngine.UI; // Nécessaire si vous manipulez des composants UI

public class QuitterJeu : MonoBehaviour
{
    // Si vous souhaitez lier le bouton via l'inspecteur
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
