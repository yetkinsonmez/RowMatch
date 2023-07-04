using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveCounter : MonoBehaviour
{
    public Grid grid;
    public int moveCount;
    public Text moveCountDisplay;
    public GameObject stageCompletedDisplay;
    private ScoreManager scoreManager;


    public static bool isGameOver = false;

    private void Start()
    {
        UpdateMoveCountDisplay();
        Debug.Log("Move : " + moveCount.ToString());

        stageCompletedDisplay.SetActive(false);
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();

    }

    public void DecreaseMoveCount()
    {
        moveCount--;
        UpdateMoveCountDisplay();

        //if(moveCount <= 0 || !grid.CheckPossibleMatches(grid.xDimension))
        if(moveCount <= 0)
        {
            Debug.Log("xDimension (MoveCounter.cs): " + grid.xDimension.ToString());
            StageCompleted();
        }
    }

    private void UpdateMoveCountDisplay()
    {
        moveCountDisplay.text = "Move\n" + moveCount;
    }

    private void StageCompleted()
    {
        isGameOver = true;

        int currentLevel = GetCurrentLevel();
        if(currentLevel >= PlayerPrefs.GetInt("levelReached", 1)) {
            PlayerPrefs.SetInt("levelReached", currentLevel + 1);
        }

        // Here we save the highest score and move count
        int currentScore = scoreManager.GetScore();
        string currentLevelStats = PlayerPrefs.GetString("LevelStats" + currentLevel, "0,0");
        string[] stats = currentLevelStats.Split(',');
        int highestScore = int.Parse(stats[0]);

        if (currentScore > highestScore)
        {
            highestScore = currentScore; // Update highest score if the current score is higher
        }

        // The format is "HighestScore,MoveCount"
        string newLevelStats = highestScore + "," + moveCount;
        PlayerPrefs.SetString("LevelStats" + currentLevel, newLevelStats);

        // Save level progress after each stage completion
        PlayerPrefs.Save();

        StartCoroutine(ShowStageCompletedDisplayAfterDelay(0.5f));        
    }

    private IEnumerator ShowStageCompletedDisplayAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        stageCompletedDisplay.SetActive(true);
    }

    private int GetCurrentLevel()
    {
        return int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));
    }

}

