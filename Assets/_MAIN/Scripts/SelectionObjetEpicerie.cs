using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script pour la sélection des objets de la liste d'épicerie
public class SelectionObjetEpicerie : MonoBehaviour
{
    /////////// Déclaration des variables pour les sons des items d'épicerie ///////////
    private AudioSource audioSource;

    // Sons pour bon ou mauvais
    public AudioClip sonMauvaisItem;
    public AudioClip sonBonItem;
    public AudioClip jeuTermine;

    // Items de la liste d'épicerie
    public AudioClip sonEpinards;
    public AudioClip sonOeufs;
    public AudioClip sonCreme;
    public AudioClip sonFarine;
    public AudioClip sonBeurre;
    public AudioClip sonTomate;
    public AudioClip sonSelPoivre;
    public AudioClip sonCitrouille;
    public AudioClip sonFromage;

    // Liste des tags des éléments de la liste d'épicerie
    // D'autres éléments seront rajoutés par la suite
    public List<string> tagsObjets = new List<string>
    { "Epinards", "Oeufs", "Creme", "Farine", "Beurre", "Tomate", "SelPoivre", "Citrouille", "Fromage" };

    // Dictionnaire contenant les données de la liste d'épicerie et les associe à un son
    // Évite de faire plusieurs conditions if
    private Dictionary<string, AudioClip> sonsParTag;

    // Dictionnaire qui identifie si un objet est bon ou mauvais
    private Dictionary<string, bool> objetsBonsOuMauvais;

    // Compteur pour le nombre total d’ingrédients bons à ramasser afin d'accorder la victoire
    private int totalBonsIngredients;
    private int bonsIngredientsRamasses = 0;

    // Pour mémoriser le dernier objet survolé afin d'éviter la répétition du son
    private string dernierObjetSurvole = "";

    public void Start()
    {
        // Déclencher l'AudioSource sur ouverture du jeu
        audioSource = GetComponent<AudioSource>();

        // Associer chaque tag à son AudioClip avec le dictionnaire
        sonsParTag = new Dictionary<string, AudioClip>
        {
            { "Epinards", sonEpinards },
            { "Oeufs", sonOeufs },
            { "Creme", sonCreme },
            { "Farine", sonFarine },
            { "Beurre", sonBeurre },
            { "Tomate", sonTomate },
            { "SelPoivre", sonSelPoivre },
            { "Citrouille", sonCitrouille },
            { "Fromage", sonFromage }
        };

        // Définition des objets bons (true) et mauvais (false)
        // Si l'objet est true dans la liste, le son de bon item jouera
        // Sinon, s'il est false, le son de mauvais item sera joué
        objetsBonsOuMauvais = new Dictionary<string, bool>
        {
            { "Epinards", true },
            { "Oeufs", true },
            { "Creme", true },
            { "Farine", true },
            { "Beurre", true },
            { "Tomate", false },
            { "SelPoivre", true },
            { "Citrouille", false },
            { "Fromage", true }
        };

        // Compter le nombre total d'ingrédients "bons" (true)
        totalBonsIngredients = 0;
        foreach (var item in objetsBonsOuMauvais)
        {
            if (item.Value)
            {
                totalBonsIngredients++;
            }
        }

        Debug.Log("Nombre total d'ingrédients bons : " + totalBonsIngredients);
    }

    void Update()
    {
        // Détection du clic de la souris
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string tagObjet = hit.collider.tag;

                if (tagsObjets.Contains(tagObjet)) // Vérifie si l'objet appartient à la liste du dictionnaire
                {
                    // Vérifie si l'objet est bon ou mauvais et joue le son correspondant
                    if (objetsBonsOuMauvais.ContainsKey(tagObjet) && objetsBonsOuMauvais[tagObjet])
                    {
                        bonsIngredientsRamasses++; // Incrémente le compteur des bons ingrédients

                        Destroy(hit.collider.gameObject); // Détruit uniquement les objets bons

                        if (sonBonItem != null)
                        {
                            audioSource.PlayOneShot(sonBonItem);
                        }

                        // Vérifie si tous les bons ingrédients sont ramassés et charge la scène suivante
                        if (bonsIngredientsRamasses >= totalBonsIngredients)
                        {
                            audioSource.PlayOneShot(jeuTermine); // Joue le son de victoire
                            ChargerSceneClasse();
                        }
                    }
                    else
                    {
                        // Les objets mauvais ne sont pas détruits mais jouent un son
                        if (sonMauvaisItem != null)
                        {
                            audioSource.PlayOneShot(sonMauvaisItem);
                        }

                        Debug.Log("Mauvais ingrédient cliqué : " + tagObjet);
                    }
                }
            }
        }

        // Détection du survol de la souris 
        Ray raySurvol = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitSurvol;

        if (Physics.Raycast(raySurvol, out hitSurvol))
        {
            string tagSurvole = hitSurvol.collider.tag;

            // Condition if afin d'éviter que le son ne joue plusieurs fois d'affilée.
            // Le dernier article survolé est mémorisé et le son ne sera rejoué que si le joueur
            // survole un autre item et revient à celui-ci
            if (sonsParTag.ContainsKey(tagSurvole) && tagSurvole != dernierObjetSurvole)
            {
                audioSource.PlayOneShot(sonsParTag[tagSurvole]);
                dernierObjetSurvole = tagSurvole; // Met à jour l'objet actuellement survolé
            }
        }
    }

    // Une fois que tous les bons ingrédients sont ramassés, la scène de classe se charge
    void ChargerSceneClasse()
    {
        Debug.Log("Tous les bons ingrédients ont été ramassés.");
        //Faire appel au script de FonduTransition.cs afin de charger la scène avec un fondu noir et éviter une coupure brusque
        FindObjectOfType<FonduTransition>().AfficherMessageEtChargerScene("NiveauIntro_ClasseRoger_MiniTest");
    }
}
