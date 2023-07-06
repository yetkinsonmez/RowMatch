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
    public ParticleSystem fireworkParticles;
    private int currentLevel;

    private ScoreManager scoreManager;
    public Grid grid;

    public AudioSource audioSource;
    public AudioClip victorySound;
    public AudioClip loseSound;
    public AudioClip clickSound;

    private void Awake()
    {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();

        playAgainButton.onClick.AddListener(() => RestartGame());
        homeButton.onClick.AddListener(() => GoToMainMenu());
        // nextLevelButton.onClick.AddListener(() => GoToNextLevel());
    }

    private void OnEnable()
    {
        grid.musicAudioSource.Stop();
        int score = scoreManager.GetScore();
        
        currentLevel = PlayerPrefs.GetInt("CurrentLevel");
        Debug.Log(currentLevel);
        
        if (score >= 1000)
        {   
            audioSource.PlayOneShot(victorySound);
            int highScore = PlayerPrefs.GetInt("Level" + currentLevel + "HighScore", 0);
            Debug.Log("Score: " + score.ToString());
            Debug.Log("Highest Score: " + highScore.ToString());
            if (score > highScore)
            {
                PlayerPrefs.SetInt("Level" + currentLevel + "HighScore", score);
            }

            totalScoreText.text = "Score: " + score;

            int highestLevel = PlayerPrefs.GetInt("highestLevel", 1);
            if (currentLevel + 1 > highestLevel)
            {
                PlayerPrefs.SetInt("highestLevel", currentLevel + 1);
            }
        
            fireworkParticles.Play();

            Sequence mySequence = DOTween.Sequence();

            mySequence.Append(transform.DOScale(1.3f, 0.6f)); 
            mySequence.Append(transform.DOScale(0.6f, 0.5f));
            mySequence.Append(transform.DOScale(1.2f, 0.4f)); 
            mySequence.Append(transform.DOScale(0.8f, 0.3f)); 
            mySequence.Append(transform.DOScale(1, 0.3f));
            
            mySequence.Play();   
        }
        else{
            audioSource.PlayOneShot(loseSound);
            totalScoreText.text = "You failed";
        }      
    }

    private void RestartGame()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // private void GoToNextLevel()
    // {
    //     audioSource.PlayOneShot(clickSound);
    //     currentLevel++;
    //     PlayerPrefs.SetInt("CurrentLevel", currentLevel);
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }


    private void GoToMainMenu()
    {
        audioSource.PlayOneShot(clickSound);
        SceneManager.LoadScene("MainMenu");
    }
}
