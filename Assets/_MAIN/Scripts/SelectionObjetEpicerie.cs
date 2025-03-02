using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Script pour la s�lection des objets de la liste d'�picerie
public class SelectionObjetEpicerie : MonoBehaviour
{
    /////////// D�claration des variables pour les sons des items d'�picerie ///////////
    private AudioSource audioSource;

    // Sons pour bon ou mauvais
    public AudioClip sonMauvaisItem;
    public AudioClip sonBonItem;
    public AudioClip jeuTermine;

    // Items de la liste d'�picerie
    public AudioClip sonEpinards;
    public AudioClip sonOeufs;
    public AudioClip sonCreme;
    public AudioClip sonFarine;
    public AudioClip sonBeurre;
    public AudioClip sonTomate;
    public AudioClip sonSelPoivre;
    public AudioClip sonCitrouille;
    public AudioClip sonFromage;

    // Liste des tags des �l�ments de la liste d'�picerie
    // D'autres �l�ments seront rajout�s par la suite
    public List<string> tagsObjets = new List<string>
    { "Epinards", "Oeufs", "Creme", "Farine", "Beurre", "Tomate", "SelPoivre", "Citrouille", "Fromage" };

    // Dictionnaire contenant les donn�es de la liste d'�picerie et les associe � un son
    // �vite de faire plusieurs conditions if
    private Dictionary<string, AudioClip> sonsParTag;

    // Dictionnaire qui identifie si un objet est bon ou mauvais
    private Dictionary<string, bool> objetsBonsOuMauvais;

    // Compteur pour le nombre total d�ingr�dients bons � ramasser afin d'accorder la victoire
    private int totalBonsIngredients;
    private int bonsIngredientsRamasses = 0;

    // Pour m�moriser le dernier objet survol� afin d'�viter la r�p�tition du son
    private string dernierObjetSurvole = "";

    public void Start()
    {
        // D�clencher l'AudioSource sur ouverture du jeu
        audioSource = GetComponent<AudioSource>();

        // Associer chaque tag � son AudioClip avec le dictionnaire
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

        // D�finition des objets bons (true) et mauvais (false)
        // Si l'objet est true dans la liste, le son de bon item jouera
        // Sinon, s'il est false, le son de mauvais item sera jou�
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

        // Compter le nombre total d'ingr�dients "bons" (true)
        totalBonsIngredients = 0;
        foreach (var item in objetsBonsOuMauvais)
        {
            if (item.Value)
            {
                totalBonsIngredients++;
            }
        }

        Debug.Log("Nombre total d'ingr�dients bons : " + totalBonsIngredients);
    }

    void Update()
    {
        // D�tection du clic de la souris
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                string tagObjet = hit.collider.tag;

                if (tagsObjets.Contains(tagObjet)) // V�rifie si l'objet appartient � la liste du dictionnaire
                {
                    // V�rifie si l'objet est bon ou mauvais et joue le son correspondant
                    if (objetsBonsOuMauvais.ContainsKey(tagObjet) && objetsBonsOuMauvais[tagObjet])
                    {
                        bonsIngredientsRamasses++; // Incr�mente le compteur des bons ingr�dients

                        Destroy(hit.collider.gameObject); // D�truit uniquement les objets bons

                        if (sonBonItem != null)
                        {
                            audioSource.PlayOneShot(sonBonItem);
                        }

                        // V�rifie si tous les bons ingr�dients sont ramass�s et charge la sc�ne suivante
                        if (bonsIngredientsRamasses >= totalBonsIngredients)
                        {
                            audioSource.PlayOneShot(jeuTermine); // Joue le son de victoire
                            ChargerSceneClasse();
                        }
                    }
                    else
                    {
                        // Les objets mauvais ne sont pas d�truits mais jouent un son
                        if (sonMauvaisItem != null)
                        {
                            audioSource.PlayOneShot(sonMauvaisItem);
                        }

                        Debug.Log("Mauvais ingr�dient cliqu� : " + tagObjet);
                    }
                }
            }
        }

        // D�tection du survol de la souris 
        Ray raySurvol = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitSurvol;

        if (Physics.Raycast(raySurvol, out hitSurvol))
        {
            string tagSurvole = hitSurvol.collider.tag;

            // Condition if afin d'�viter que le son ne joue plusieurs fois d'affil�e.
            // Le dernier article survol� est m�moris� et le son ne sera rejou� que si le joueur
            // survole un autre item et revient � celui-ci
            if (sonsParTag.ContainsKey(tagSurvole) && tagSurvole != dernierObjetSurvole)
            {
                audioSource.PlayOneShot(sonsParTag[tagSurvole]);
                dernierObjetSurvole = tagSurvole; // Met � jour l'objet actuellement survol�
            }
        }
    }

    // Une fois que tous les bons ingr�dients sont ramass�s, la sc�ne de classe se charge
    void ChargerSceneClasse()
    {
        Debug.Log("Tous les bons ingr�dients ont �t� ramass�s.");
        //Faire appel au script de FonduTransition.cs afin de charger la sc�ne avec un fondu noir et �viter une coupure brusque
        FindObjectOfType<FonduTransition>().AfficherMessageEtChargerScene("NiveauIntro_ClasseRoger_MiniTest");
    }
}
