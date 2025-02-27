using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class JeuTraduction : MonoBehaviour
{
    // Variables pour les questions et les r�ponses du jeu de traduction
    [System.Serializable]
    public class TraductionQuestion
    {
        public string phraseATraduire; // Mot ou phrase � traduire
        public string bonneReponse; // Bonne r�ponse
        public string[] options; // Choix de r�ponses
    }

    public TextMeshProUGUI questionTexte; // Texte qui affiche la question
    public Button[] boutonsReponses; // Il y aura � l'�cran 3 boutons pour les choix de r�ponse.
    public TextMeshProUGUI reponseText; // Texte pour afficher le r�sultat de la question courante
    public Image imageFin; // Image � afficher � la fin du jeu

    public TraductionQuestion[] questions; // Tableau contenant toutes les questions possibles (� d�finir dans l'inspecteur Unity)
    private List<int> questionsRestantes = new List<int>(); // Liste des indices de questions non pos�es
    private int questionsReussies = 0; // Compteur des questions r�ussies
    private TraductionQuestion questionCourante; // Stocke la question actuelle

    void Start()
    {
        if (questions == null || questions.Length == 0)
        {
            Debug.LogError("Aucune question d�finie dans l'inspecteur.");
            return;
        }
        imageFin.gameObject.SetActive(false); // Cacher l'image de fin au d�but

        // Remplir la liste des questions restantes
        for (int i = 0; i < questions.Length; i++)
        {
            questionsRestantes.Add(i);
        }

        ChargerQuestion(); // Charger une premi�re question au d�marrage
    }

    void ChargerQuestion()
    {
        if (questionsRestantes.Count == 0)
        {
            FinDuJeu();
            return;
        }

        int randomIndex = Random.Range(0, questionsRestantes.Count);
        int indexQuestionCourante = questionsRestantes[randomIndex];
        questionsRestantes.RemoveAt(randomIndex); // Supprime la question de la liste
        questionCourante = questions[indexQuestionCourante];

        questionTexte.text = questionCourante.phraseATraduire; // Afficher la phrase � traduire
        reponseText.text = ""; // R�initialiser le texte du r�sultat

        // Utiliser un HashSet pour garantir l'unicit� des r�ponses
        HashSet<string> optionsUniques = new HashSet<string>(questionCourante.options);
        optionsUniques.Add(questionCourante.bonneReponse);

        List<string> optionsMelangees = new List<string>(optionsUniques);
        optionsMelangees.Shuffle(); // M�langer les r�ponses

        for (int i = 0; i < boutonsReponses.Length; i++)
        {
            if (i < optionsMelangees.Count) // V�rifie qu'il y a assez d'options
            {
                boutonsReponses[i].GetComponentInChildren<TextMeshProUGUI>().text = optionsMelangees[i]; // Mettre � jour le texte du bouton
                boutonsReponses[i].onClick.RemoveAllListeners(); // Supprimer les anciens �v�nements de clic

                // V�rifier si c'est la bonne r�ponse
                bool estBonneReponse = optionsMelangees[i] == questionCourante.bonneReponse;
                boutonsReponses[i].onClick.AddListener(() => VerifierReponse(estBonneReponse));
            }
            else
            {
                boutonsReponses[i].gameObject.SetActive(false); // Masquer le bouton s'il n'y a pas assez d'options
            }
        }
    }


    // V�rifier si la r�ponse s�lectionn�e est correcte
    void VerifierReponse(bool isCorrect)
    {
        // D�finir les couleurs pour les r�ponses
        string bonneCouleur = "<color=green>"; // Couleur pour la bonne r�ponse (vert)
        string mauvaiseCouleur = "<color=red>"; // Couleur pour la mauvaise r�ponse (rouge)
        string finCouleur = "</color>"; // Balise de fermeture de couleur

        // Afficher le r�sultat avec la couleur appropri�e
        reponseText.text = isCorrect ? bonneCouleur + "Bonne r�ponse !" + finCouleur : mauvaiseCouleur + "T'es pas s�rieuse ..." + finCouleur;

        if (isCorrect)
        {
            questionsReussies++;
            Invoke("ChargerQuestion", 1.5f); // Attendre 1.5 secondes avant la prochaine question
        }
    }

    // Fonction pour terminer le jeu lorsque toutes les questions ont �t� r�pondues correctement
    // Afficher l'image de fin et le message
    void FinDuJeu()
    {
        questionTexte.text = "Bravo ! C'est bien r�ussi !";
        reponseText.text = "";

        // D�sactiver tous les boutons de choix de r�ponses
        foreach (var bouton in boutonsReponses)
        {
            bouton.gameObject.SetActive(false); //Mettre l'interaction des boutons � False; pour d�sactiver les interactions
        }

        imageFin.gameObject.SetActive(true); //Une image de quiche s'affiche pour f�liciter le joueur
        Invoke("ChargerSceneFin", 3f); // Attendre 3 secondes avant de changer de sc�ne
    }

    // Charger la sc�ne de fin
    void ChargerSceneFin()
    {
        SceneManager.LoadScene("FinJeu");
    }
}

public static class ListExtensions
{
    //M�thode pour m�langer les �l�ments du tableau afin d'�viter que les questions ne se r�p�tent
    public static void Shuffle<T>(this List<T> list)
    {
        //Condition for pour parcourir tous les �l�ments de la liste en commen�ant par le dernier
        for (int i = list.Count - 1; i > 0; i--)
        {
            //G�n�re un index al�atoire
            int randomIndex = Random.Range(0, i + 1);
            
            //M�lange les �l�ments
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
