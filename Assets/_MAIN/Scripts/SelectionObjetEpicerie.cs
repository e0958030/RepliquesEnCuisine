using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Script pour la sélection des objets de la liste d'épicerie
public class SelectionObjetEpicerie : MonoBehaviour
{
    /////////// Déclaration des variables pour les sons des items d'épicerie ///////////
    private AudioSource audioSource;

    //Sons pour bon ou mauvais
    public AudioClip sonMauvaisItem;
    public AudioClip sonBonItem;

    //Items de la liste d'épicerie
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
    public List<string> tagsObjets = new List<string> { "Epinards", "Oeufs", "Creme", "Farine", "Beurre", "Tomate", "SelPoivre", "Citrouille", "Fromage" };

    //Dictionnaire contenant les données de la liste d'épicerie et les associe à un son
    //Évite de faire plusieurs conditions if
    private Dictionary<string, AudioClip> sonsParTag;

    public void Start()
    {
        //Déclencher l'AudioSource sur ouverture du jeu
        audioSource = GetComponent<AudioSource>();

        //Associer chaque tag à son AudioClip avec le dictionnaire
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

                if (tagsObjets.Contains(tagObjet)) // Vérifie si l'objet appartient à la liste
                {
                    Destroy(hit.collider.gameObject);
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

        // Détection du survol de la souris (toujours actif dans Update)
        Ray raySurvol = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitSurvol;

        if (Physics.Raycast(raySurvol, out hitSurvol))
        {
            string tagSurvole = hitSurvol.collider.tag;

            if (sonsParTag.ContainsKey(tagSurvole))
            {
                if (!audioSource.isPlaying) // Joue le son uniquement si rien n'est en train de jouer
                {
                    audioSource.PlayOneShot(sonsParTag[tagSurvole]);
                }
            }
        }
    }
}
