using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JeuTraduction : MonoBehaviour
{
    [System.Serializable]

    //Tableau pour les questions et réponses
    public class TranslationQuestion
    {
        public string phraseATraduire;
        public string bonneReponse;
        public string[] options;
    }

    public TextMeshProUGUI questionTexte;
    public Button[] boutonsReponses;
    public TextMeshProUGUI reponseText;

    public TranslationQuestion[] questions;
    private int IndexQuestionCourante;

    void Start()
    {
        ChargerQuestion();
    }

    void ChargerQuestion() 
    {
        if (questions.Length == 0) return;

        IndexQuestionCourante = Random.Range(0, questions.Length);
        TranslationQuestion questionCourante = questions[IndexQuestionCourante];

        questionTexte.text = questionCourante.phraseATraduire;
        reponseText.text = "";

        int correctAnswerIndex = Random.Range(0, boutonsReponses.Length);

        for (int i = 0; i < boutonsReponses.Length; i++)
        {
            int optionIndex = (i == correctAnswerIndex) ? 0 : i + 1;
            boutonsReponses[i].GetComponentInChildren<TextMeshProUGUI>().text = questionCourante.options[optionIndex];
            boutonsReponses[i].onClick.RemoveAllListeners();

            if (i == correctAnswerIndex)
                boutonsReponses[i].onClick.AddListener(() => CheckAnswer(true));
            else
                boutonsReponses[i].onClick.AddListener(() => CheckAnswer(false));
        }
    }

    void CheckAnswer(bool isCorrect)
    {
        reponseText.text = isCorrect ? "Correct!" : "Wrong! Try again.";
    }
}
