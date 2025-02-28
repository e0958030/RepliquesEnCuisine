using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

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

    //Variables de son sur clic des boutons
    private AudioSource audioSource;

    //Sons pour la bonne ou mauvaise réponse
    public AudioClip sonMauvaiseReponse;
    public AudioClip sonBonneReponse;
    public AudioClip sonVictoire;

    //Variables pour désactiver le dialogue et activer le mini-jeu
    public GameObject dialogueUI; // Contiendra l'image et le dialogue
    public GameObject miniJeu; // Référence au GameObject du mini-jeu
    public Button skipButton; // Bouton pour passer le dialogue

    //Variables texte
    public TextMeshProUGUI questionTexte; // Texte qui affiche la question
    public Button[] boutonsReponses; // Il y aura à l'écran 3 boutons pour les choix de réponse.
    public TextMeshProUGUI reponseText; // Texte pour afficher le résultat de la question courante
    public Image imageFin; // Image à afficher à la fin du jeu

    //Variables questions
    public TraductionQuestion[] questions; // Tableau contenant toutes les questions possibles (à définir dans l'inspecteur Unity)
    private List<int> questionsRestantes = new List<int>(); // Liste des indices de questions non posées
    private int questionsReussies = 0; // Compteur des questions réussies
    private TraductionQuestion questionCourante; // Stocke la question actuelle

    private Coroutine attenteCoroutine; // Stocker la coroutine pour pouvoir la stopper
    void Start()
    {
        // Déclencher l'AudioSource sur ouverture du jeu
        audioSource = GetComponent<AudioSource>();

        miniJeu.SetActive(false); // Désactiver le mini-jeu au début
        skipButton.onClick.AddListener(SauterDialogue); // Associer l'événement au bouton
        //StartCoroutine(AttendreEtLancerJeu()); // Lancer le compteur de 30 secondes, le dialogue et l'image se désactiveront à la fin et le quiz s'affichera

        IEnumerator AttendreEtLancerJeu()
        {
            yield return new WaitForSeconds(30f); // Attendre 30 secondes avant de désactiver le dialogue et la feuille d'instructions
            LancerMiniJeu();
        }

        //Bouton pour sauter le dialogue sans avoir à attendre les 30 secondes
        void SauterDialogue()
        {
            StopCoroutine(AttendreEtLancerJeu()); // Arrêter la coroutine si le joueur clique
            LancerMiniJeu();
        }

        //Fonction pour désactiver les instructions et activer le canvas contenant le jeu
        void LancerMiniJeu()
        {
            dialogueUI.SetActive(false); // Désactiver le dialogue et l'image
            miniJeu.SetActive(true); // Activer le mini-jeu
            skipButton.gameObject.SetActive(false); // Désactiver le bouton pour sauter les instructions
        }

        //Le mini jeu des questions
        if (questions == null || questions.Length == 0)
        {
            //Debug.LogError("Aucune question définie dans l'inspecteur.");
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
    
    //Fonction qui va générer aléatoirement les questions et leurs choix de réponses définies dans l'inspecteur.
    void ChargerQuestion()
    {
        if (questionsRestantes.Count == 0)
        {
            FinDuJeu();
            return;
        }

        int randomIndex = Random.Range(0, questionsRestantes.Count);
        int indexQuestionCourante = questionsRestantes[randomIndex];
        questionsRestantes.RemoveAt(randomIndex); // Supprime la question de la liste comme elle a été déjà répondue
        questionCourante = questions[indexQuestionCourante];

        questionTexte.text = questionCourante.phraseATraduire; // Afficher la phrase à traduire
        reponseText.text = ""; // Réinitialiser le texte du résultat

        //Utiliser un HashSet pour s'assurer que les choix de réponse ne se répètent pas
        //Collection C# qui trie les éléments d'une liste de manière non ordonnée afin d'éviter les répétitions.
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
        string finCouleur = "</color>"; 

        // Afficher le résultat avec la couleur appropriée
        reponseText.text = isCorrect ? bonneCouleur + "Bonne réponse !" + finCouleur : mauvaiseCouleur + "T'es pas sérieuse ..." + finCouleur;

        //Si la réponse choisie est correcte 
        if (isCorrect)
        {
            questionsReussies++; //Incrémenter le compteur de bonnes réponses accumulées
            audioSource.PlayOneShot(sonBonneReponse); //Jouer un son de rétroaction
            Invoke("ChargerQuestion", 1.5f); // Attendre 1.5 secondes avant la prochaine question
        }

        //Si la réponse n'est pas correcte
        if (!isCorrect)
        {
            audioSource.PlayOneShot(sonMauvaiseReponse); //Jouer un son de rétroaction
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
        audioSource.PlayOneShot(sonVictoire); //Jouer le son de victoire
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
