using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class EndGameController : MonoBehaviour
{
    public Text totalScoreText;
    public Button playAgainButton;
    public Button homeButton;
    public Button nextLevelButton;

    private ScoreManager scoreManager;
    private int currentLevel = 1; // set this based on your current level

    private void Awake()
    {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        playAgainButton.onClick.AddListener(() => RestartGame());
        homeButton.onClick.AddListener(() => GoToMainMenu());
        nextLevelButton.onClick.AddListener(() => GoToNextLevel());
    }

    private void OnEnable()
    {
        // populate score text with the final score
        totalScoreText.text = "Score: " + scoreManager.GetScore();

        Sequence mySequence = DOTween.Sequence();

        mySequence.Append(transform.DOScale(1.3f, 0.4f)); 
        mySequence.Append(transform.DOScale(0.6f, 0.3f));
        mySequence.Append(transform.DOScale(1.2f, 0.3f)); 
        mySequence.Append(transform.DOScale(0.8f, 0.2f)); 
        mySequence.Append(transform.DOScale(1, 0.2f));

        mySequence.Play(); 
    }

    private void RestartGame()
    {
        int highScore = PlayerPrefs.GetInt("Level" + currentLevel + "HighScore", 0);
        if (scoreManager.GetScore() > highScore)
        {
            PlayerPrefs.SetInt("Level" + currentLevel + "HighScore", scoreManager.GetScore());
        }

        SceneManager.LoadScene("Level" + currentLevel);
    }

    // Update the GoToNextLevel() method
    private void GoToNextLevel()
    {
        int highScore = PlayerPrefs.GetInt("Level" + currentLevel + "HighScore", 0);
        if (scoreManager.GetScore() > highScore)
        {
            PlayerPrefs.SetInt("Level" + currentLevel + "HighScore", scoreManager.GetScore());
        }

        currentLevel++;
        SceneManager.LoadScene("Level" + currentLevel);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
