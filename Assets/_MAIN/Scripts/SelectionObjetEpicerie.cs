using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script pour la s�lection des objets de la liste d'�picerie
public class SelectionObjetEpicerie : MonoBehaviour
{
    /////////// D�claration des variables pour les sons des items d'�picerie ///////////
    private AudioSource audioSource;

    // Sons pour bon ou mauvais
    public AudioClip sonMauvaisItem;
    public AudioClip sonBonItem;

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
        objetsBonsOuMauvais = new Dictionary<string, bool>
        {
            { "Epinards", true },   // Bon
            { "Oeufs", true },      // Bon
            { "Creme", true },     // Bon
            { "Farine", true },    // Bon
            { "Beurre", true },    // Bon
            { "Tomate", false },     // Mauvais
            { "SelPoivre", true },  // Bon
            { "Citrouille", false },// Mauvais
            { "Fromage", true }     // Bon
        };
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

                if (tagsObjets.Contains(tagObjet)) // V�rifie si l'objet appartient � la liste
                {
                    Destroy(hit.collider.gameObject); // D�truit l'objet cliqu�

                    // V�rifie si l'objet est bon ou mauvais et joue le son correspondant
                    if (objetsBonsOuMauvais.ContainsKey(tagObjet) && objetsBonsOuMauvais[tagObjet])
                    {
                        if (sonBonItem != null)
                        {
                            audioSource.PlayOneShot(sonBonItem);
                        }
                    }
                    else
                    {
                        if (sonMauvaisItem != null)
                        {
                            audioSource.PlayOneShot(sonMauvaisItem);
                        }
                    }
                }
            }
        }

        // D�tection du survol de la souris (toujours actif dans Update)
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
}
