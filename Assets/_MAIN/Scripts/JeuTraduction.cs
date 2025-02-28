using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

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

    //Variables de son sur clic des boutons
    private AudioSource audioSource;

    //Sons pour la bonne ou mauvaise r�ponse
    public AudioClip sonMauvaiseReponse;
    public AudioClip sonBonneReponse;
    public AudioClip sonVictoire;

    //Variables pour d�sactiver le dialogue et activer le mini-jeu
    public GameObject dialogueUI; // Contiendra l'image et le dialogue
    public GameObject miniJeu; // R�f�rence au GameObject du mini-jeu
    public Button skipButton; // Bouton pour passer le dialogue

    //Variables texte
    public TextMeshProUGUI questionTexte; // Texte qui affiche la question
    public Button[] boutonsReponses; // Il y aura � l'�cran 3 boutons pour les choix de r�ponse.
    public TextMeshProUGUI reponseText; // Texte pour afficher le r�sultat de la question courante
    public Image imageFin; // Image � afficher � la fin du jeu

    //Variables questions
    public TraductionQuestion[] questions; // Tableau contenant toutes les questions possibles (� d�finir dans l'inspecteur Unity)
    private List<int> questionsRestantes = new List<int>(); // Liste des indices de questions non pos�es
    private int questionsReussies = 0; // Compteur des questions r�ussies
    private TraductionQuestion questionCourante; // Stocke la question actuelle

    private Coroutine attenteCoroutine; // Stocker la coroutine pour pouvoir la stopper
    void Start()
    {
        // D�clencher l'AudioSource sur ouverture du jeu
        audioSource = GetComponent<AudioSource>();

        miniJeu.SetActive(false); // D�sactiver le mini-jeu au d�but
        skipButton.onClick.AddListener(SauterDialogue); // Associer l'�v�nement au bouton
        //StartCoroutine(AttendreEtLancerJeu()); // Lancer le compteur de 30 secondes, le dialogue et l'image se d�sactiveront � la fin et le quiz s'affichera

        IEnumerator AttendreEtLancerJeu()
        {
            yield return new WaitForSeconds(30f); // Attendre 30 secondes avant de d�sactiver le dialogue et la feuille d'instructions
            LancerMiniJeu();
        }

        //Bouton pour sauter le dialogue sans avoir � attendre les 30 secondes
        void SauterDialogue()
        {
            StopCoroutine(AttendreEtLancerJeu()); // Arr�ter la coroutine si le joueur clique
            LancerMiniJeu();
        }

        //Fonction pour d�sactiver les instructions et activer le canvas contenant le jeu
        void LancerMiniJeu()
        {
            dialogueUI.SetActive(false); // D�sactiver le dialogue et l'image
            miniJeu.SetActive(true); // Activer le mini-jeu
            skipButton.gameObject.SetActive(false); // D�sactiver le bouton pour sauter les instructions
        }

        //Le mini jeu des questions
        if (questions == null || questions.Length == 0)
        {
            //Debug.LogError("Aucune question d�finie dans l'inspecteur.");
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
    
    //Fonction qui va g�n�rer al�atoirement les questions et leurs choix de r�ponses d�finies dans l'inspecteur.
    void ChargerQuestion()
    {
        if (questionsRestantes.Count == 0)
        {
            FinDuJeu();
            return;
        }

        int randomIndex = Random.Range(0, questionsRestantes.Count);
        int indexQuestionCourante = questionsRestantes[randomIndex];
        questionsRestantes.RemoveAt(randomIndex); // Supprime la question de la liste comme elle a �t� d�j� r�pondue
        questionCourante = questions[indexQuestionCourante];

        questionTexte.text = questionCourante.phraseATraduire; // Afficher la phrase � traduire
        reponseText.text = ""; // R�initialiser le texte du r�sultat

        //Utiliser un HashSet pour s'assurer que les choix de r�ponse ne se r�p�tent pas
        //Collection C# qui trie les �l�ments d'une liste de mani�re non ordonn�e afin d'�viter les r�p�titions.
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
        string finCouleur = "</color>"; 

        // Afficher le r�sultat avec la couleur appropri�e
        reponseText.text = isCorrect ? bonneCouleur + "Bonne r�ponse !" + finCouleur : mauvaiseCouleur + "T'es pas s�rieuse ..." + finCouleur;

        //Si la r�ponse choisie est correcte 
        if (isCorrect)
        {
            questionsReussies++; //Incr�menter le compteur de bonnes r�ponses accumul�es
            audioSource.PlayOneShot(sonBonneReponse); //Jouer un son de r�troaction
            Invoke("ChargerQuestion", 1.5f); // Attendre 1.5 secondes avant la prochaine question
        }

        //Si la r�ponse n'est pas correcte
        if (!isCorrect)
        {
            audioSource.PlayOneShot(sonMauvaiseReponse); //Jouer un son de r�troaction
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
        audioSource.PlayOneShot(sonVictoire); //Jouer le son de victoire
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
