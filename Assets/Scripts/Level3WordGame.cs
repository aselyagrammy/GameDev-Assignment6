using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Level3WordGame : MonoBehaviour
{
    public TextMeshProUGUI scrambledWordText;
    public TMP_InputField inputField;
    public TextMeshProUGUI feedbackText;
    public TextMeshProUGUI scoreText;
    public Button submitButton;
    public Button nextButton;

    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioSource audioSource;

    public WordQuestion[] questions;

    private int currentQuestionIndex = 0;
    private int score = 0;

    void Start()
    {
        if(audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("Missing AudioSource component!");
            }
        }

        ShowQuestion();
        submitButton.onClick.AddListener(CheckAnswer);
        nextButton.onClick.AddListener(NextQuestion);
    }

    void ShowQuestion()
    {
        inputField.text = "";
        feedbackText.text = "";
        scrambledWordText.text = questions[currentQuestionIndex].scrambledWord;
        nextButton.gameObject.SetActive(false);
    }

    void CheckAnswer()
    {
        string userAnswer = inputField.text.Trim().ToLower();
        string correctAnswer = questions[currentQuestionIndex].correctWord.ToLower();

        if (userAnswer == correctAnswer)
        {
            feedbackText.text = "Correct!";
            score++;
            if (correctSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(correctSound);
            }
        }
        else
        {
            feedbackText.text = "Not Correct!";
            if (wrongSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(wrongSound);
            }
        }

        scoreText.text = "Score: " + score;
        nextButton.gameObject.SetActive(true);
        submitButton.interactable = false;
        inputField.interactable = false;
    }

    void NextQuestion()
    {
        currentQuestionIndex++;
        submitButton.interactable = true;
        inputField.interactable = true;

        if (currentQuestionIndex < questions.Length)
            ShowQuestion();
        else
            EndGame();
    }

    void EndGame()
    {
        scrambledWordText.text = "Game Complete! Final score "+ score;
        feedbackText.text = "";
        inputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }
}
