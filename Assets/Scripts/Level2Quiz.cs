using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Level2Quiz : MonoBehaviour
{
    public AudioClip correctAnswerSound;
    public AudioClip wrongAnswerSound;
    public AudioSource audioSource;

    public TextMeshProUGUI questionText;
    public Button[] optionButtons; 
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI scoreText;

    public TrueFalseQuestion[] questions;

    private int currentQuestionIndex = 0;
    private int score = 0;
    private float correctAnswerDelay = 1.5f;
    private bool waitingForNextQuestion = false;
    private float nextQuestionTime;

    void Start()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("Missing AudioSource component!");
            }
        }

        optionButtons[0].onClick.AddListener(() => CheckAnswer(true));
        optionButtons[1].onClick.AddListener(() => CheckAnswer(false));
        

        if (questions == null || questions.Length == 0)
        {
            Debug.LogError("No questions assigned!");
            return;
        }

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
        questionText.text = questions[currentQuestionIndex].questionText;

        optionButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "True";
        optionButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "False";

        foreach (var btn in optionButtons)
        {
            btn.interactable = true;
        }
    }

    void CheckAnswer(bool answer)
    {
        bool correct = questions[currentQuestionIndex].isTrue;

        foreach (var btn in optionButtons)
        {
            btn.interactable = false;
        }

        if (answer == correct)
        {
            feedbackText.text = "Correct!";
            score++;

            if (correctAnswerSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(correctAnswerSound);
            }
        }
        else
        {
            feedbackText.text = "Not Correct!";

            if (wrongAnswerSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(wrongAnswerSound);
            }
        }

        scoreText.text = "Score: " + score;

        waitingForNextQuestion = true;
        nextQuestionTime = Time.time + correctAnswerDelay;
    }

    public void OnNextQuestion()
    {
        currentQuestionIndex++;
        waitingForNextQuestion = false;

        if (currentQuestionIndex < questions.Length)
        {
            ShowQuestion();
        }
        else
        {
            EndQuiz();
        }
    }

    void EndQuiz()
    {
        questionText.text = "Quiz Complete! Final Score: " + score;
        feedbackText.text = "";
        foreach (var btn in optionButtons)
        {
            btn.gameObject.SetActive(false);
        }
        
    }
}

