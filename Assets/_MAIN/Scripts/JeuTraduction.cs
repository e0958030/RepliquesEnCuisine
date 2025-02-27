using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class JeuTraduction : MonoBehaviour
{
    // Variables pour les questions et les réponses du jeu de traduction
    [System.Serializable]
    public class TraductionQuestion
    {
        public string phraseATraduire; // Mot ou phrase à traduire
        public string bonneReponse; // Bonne réponse
        public string[] options; // Choix de réponses
    }

    public TextMeshProUGUI questionTexte; // Texte qui affiche la question
    public Button[] boutonsReponses; // Il y aura à l'écran 3 boutons pour les choix de réponse.
    public TextMeshProUGUI reponseText; // Texte pour afficher le résultat de la question courante
    public Image imageFin; // Image à afficher à la fin du jeu

    public TraductionQuestion[] questions; // Tableau contenant toutes les questions possibles (à définir dans l'inspecteur Unity)
    private List<int> questionsRestantes = new List<int>(); // Liste des indices de questions non posées
    private int questionsReussies = 0; // Compteur des questions réussies
    private TraductionQuestion questionCourante; // Stocke la question actuelle

    void Start()
    {
        if (questions == null || questions.Length == 0)
        {
            Debug.LogError("Aucune question définie dans l'inspecteur.");
            return;
        }
        imageFin.gameObject.SetActive(false); // Cacher l'image de fin au début

        // Remplir la liste des questions restantes
        for (int i = 0; i < questions.Length; i++)
        {
            questionsRestantes.Add(i);
        }

        ChargerQuestion(); // Charger une première question au démarrage
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

        questionTexte.text = questionCourante.phraseATraduire; // Afficher la phrase à traduire
        reponseText.text = ""; // Réinitialiser le texte du résultat

        // Utiliser un HashSet pour garantir l'unicité des réponses
        HashSet<string> optionsUniques = new HashSet<string>(questionCourante.options);
        optionsUniques.Add(questionCourante.bonneReponse);

        List<string> optionsMelangees = new List<string>(optionsUniques);
        optionsMelangees.Shuffle(); // Mélanger les réponses

        for (int i = 0; i < boutonsReponses.Length; i++)
        {
            if (i < optionsMelangees.Count) // Vérifie qu'il y a assez d'options
            {
                boutonsReponses[i].GetComponentInChildren<TextMeshProUGUI>().text = optionsMelangees[i]; // Mettre à jour le texte du bouton
                boutonsReponses[i].onClick.RemoveAllListeners(); // Supprimer les anciens événements de clic

                // Vérifier si c'est la bonne réponse
                bool estBonneReponse = optionsMelangees[i] == questionCourante.bonneReponse;
                boutonsReponses[i].onClick.AddListener(() => VerifierReponse(estBonneReponse));
            }
            else
            {
                boutonsReponses[i].gameObject.SetActive(false); // Masquer le bouton s'il n'y a pas assez d'options
            }
        }
    }


    // Vérifier si la réponse sélectionnée est correcte
    void VerifierReponse(bool isCorrect)
    {
        // Définir les couleurs pour les réponses
        string bonneCouleur = "<color=green>"; // Couleur pour la bonne réponse (vert)
        string mauvaiseCouleur = "<color=red>"; // Couleur pour la mauvaise réponse (rouge)
        string finCouleur = "</color>"; // Balise de fermeture de couleur

        // Afficher le résultat avec la couleur appropriée
        reponseText.text = isCorrect ? bonneCouleur + "Bonne réponse !" + finCouleur : mauvaiseCouleur + "T'es pas sérieuse ..." + finCouleur;

        if (isCorrect)
        {
            questionsReussies++;
            Invoke("ChargerQuestion", 1.5f); // Attendre 1.5 secondes avant la prochaine question
        }
    }

    // Fonction pour terminer le jeu lorsque toutes les questions ont été répondues correctement
    // Afficher l'image de fin et le message
    void FinDuJeu()
    {
        questionTexte.text = "Bravo ! C'est bien réussi !";
        reponseText.text = "";

        // Désactiver tous les boutons de choix de réponses
        foreach (var bouton in boutonsReponses)
        {
            bouton.gameObject.SetActive(false); //Mettre l'interaction des boutons à False; pour désactiver les interactions
        }

        imageFin.gameObject.SetActive(true); //Une image de quiche s'affiche pour féliciter le joueur
        Invoke("ChargerSceneFin", 3f); // Attendre 3 secondes avant de changer de scène
    }

    // Charger la scène de fin
    void ChargerSceneFin()
    {
        SceneManager.LoadScene("FinJeu");
    }
}

public static class ListExtensions
{
    //Méthode pour mélanger les éléments du tableau afin d'éviter que les questions ne se répètent
    public static void Shuffle<T>(this List<T> list)
    {
        //Condition for pour parcourir tous les éléments de la liste en commençant par le dernier
        for (int i = list.Count - 1; i > 0; i--)
        {
            //Génère un index aléatoire
            int randomIndex = Random.Range(0, i + 1);
            
            //Mélange les éléments
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
