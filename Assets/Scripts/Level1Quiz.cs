using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Level1Quiz : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button[] optionButtons;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI scoreText;
    public Button nextButton;

    public Question[] questions;


    public AudioSource audioSource;
    public AudioClip correctSound;          
    public AudioClip wrongSound;

    private int currentQuestionIndex = 0;
    private int score = 0;
    private float correctAnswerDelay = 1.5f; 
    private bool waitingForNextQuestion = false;
    private float nextQuestionTime;

    void Start()
    {
        ShowQuestion();
    }

    void Update()
    {

        if (waitingForNextQuestion && Time.time >= nextQuestionTime)
        {
            waitingForNextQuestion = false;
            OnNextQuestion();
        }
    }

    void ShowQuestion()
    {
        feedbackText.text = "";
        Question q = questions[currentQuestionIndex];
        questionText.text = q.questionText;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            int index = i;
            optionButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = q.options[i];
            optionButtons[i].onClick.RemoveAllListeners();
            optionButtons[i].onClick.AddListener(() => CheckAnswer(index));
            optionButtons[i].interactable = true;
        }
    }

    void CheckAnswer(int index)
    {
        Question q = questions[currentQuestionIndex];

        
        foreach (var btn in optionButtons)
        {
            btn.interactable = false;
        }

        if (index == q.correctAnswerIndex)
        {
            feedbackText.text = "Correct!";
            feedbackText.color = Color.black;
            score++;
            audioSource.PlayOneShot(correctSound); 


        }
        else
        {
            feedbackText.text = "Not Correct!";
            audioSource.PlayOneShot(wrongSound);

        }

        
        waitingForNextQuestion = true;
        nextQuestionTime = Time.time + correctAnswerDelay;

        scoreText.text = "Score: " + score;
    }

    public void OnNextQuestion()
    {
        currentQuestionIndex++;
        nextButton.gameObject.SetActive(false);
        waitingForNextQuestion = false; 

        if (currentQuestionIndex < questions.Length)
            ShowQuestion();
        else
            EndQuiz();
    }

    void EndQuiz()
    {
        questionText.text = "Quiz Complete! Final Score " + score;
        feedbackText.text = "";
        foreach (var btn in optionButtons)
            btn.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }
}