using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CliquerObjet3D : MonoBehaviour
{
    //Déclaration des variables

    // Pour référer aux noms des scènes
    public string nomSceneAppart = "NiveauIntro_Appart";

    // Musique et effets sonores
    private AudioSource audioSource;
    public AudioClip sonClic; // Pour la rétroaction sur le clic de souris
    private float delaiAvantChangement = 2f; // Délai de 2 secondes

    // Lieux de la carte de la ville
    public GameObject appartement;
    public GameObject gare;
    public GameObject ecole;
    public GameObject epicerie;
    public GameObject cafe;

    void Start()
    {
        // Récupérer l'AudioSource attaché à l'objet
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Détecter que l'objet a été cliqué
        if (Input.GetMouseButtonDown(0))
        {
            if (appartement == GetClickedObject(out RaycastHit hit))
            {
                // Jouer le son du clic
                if (audioSource && sonClic)
                {
                    //audioSource.PlayOneShot(sonClic);
                }

                // Démarrer la coroutine pour attendre avant de changer de scène
                StartCoroutine(AttendreEtChangerScene());
            }
        }
    }

    // Coroutine pour attendre avant de changer de scène
    IEnumerator AttendreEtChangerScene()
    {
        yield return new WaitForSeconds(delaiAvantChangement);
        SceneManager.LoadScene(nomSceneAppart);
    }

    // Code de détection de clic sur l'objet visé et non sur le canvas
    GameObject GetClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            if (!isPointerOverUIObject())
            {
                target = hit.collider.gameObject;
            }
        }
        return target;
    }

    private bool isPointerOverUIObject()
    {
        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);
        return results.Count > 0;
    }

    // Ajouter un effet de hover sur les objets lorsque la souris le survole
    private void OnMouseEnter()
    {
        AugmenterTaille(true);
    }

    private void OnMouseExit()
    {
        AugmenterTaille(false);
    }

    private Vector3 tailleInitiale;

    private void Awake()
    {
        tailleInitiale = transform.localScale;
    }

    // Augmenter la taille
    private void AugmenterTaille(bool status)
    {
        Vector3 tailleFinale = tailleInitiale;

        // Si la condition est vraie, la taille augmente
        if (status)
            tailleFinale = tailleInitiale * 1.1f;

        transform.localScale = tailleFinale;
    }
}
