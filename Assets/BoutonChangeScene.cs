using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoutonChangeScene : MonoBehaviour
{
    /////// Déclaration des variables ///////
    public AudioSource audio;
    public string sceneSuivante; // Nom de la scène à charger

    public void boutonSuivant()
    {
        if (audio != null)
        {
            audio.Play();
            StartCoroutine(AttendreEtChangerScene());
        }
        else
        {
            ChangerScene();
        }
    }

    private IEnumerator AttendreEtChangerScene()
    {
        //Attendre la durée du son 
        yield return new WaitForSeconds(audio.clip.length/2);
        ChangerScene();
    }

    private void ChangerScene()
    {
        SceneManager.LoadScene("Avis");
    }
}

