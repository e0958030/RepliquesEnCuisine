using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoutonChangeScene : MonoBehaviour
{
    /////// D�claration des variables ///////
    public AudioSource audio;
    public string sceneSuivante; // Nom de la sc�ne � charger
    public string sceneCredits; //Pour la scene des credits

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
        //Attendre la dur�e du son 
        yield return new WaitForSeconds(audio.clip.length/2);
        ChangerScene();
    }

    private void ChangerScene()
    {
        SceneManager.LoadScene(sceneSuivante);
    }
}

