using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    }

    private void RestartGame()
    {
        // load the same level
        SceneManager.LoadScene("Level" + currentLevel);
    }

    private void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void GoToNextLevel()
    {
        currentLevel++;
        SceneManager.LoadScene("Level" + currentLevel);
    }
}
