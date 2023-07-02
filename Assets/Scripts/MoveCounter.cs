using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoveCounter : MonoBehaviour
{
    public Grid grid;
    public int moveCount = 20;
    public Text moveCountDisplay;
    public GameObject stageCompletedDisplay;

    public static bool isGameOver = false;

    private void Start()
    {
        UpdateMoveCountDisplay();
        stageCompletedDisplay.SetActive(false);
    }

    public void DecreaseMoveCount()
    {
        moveCount--;
        UpdateMoveCountDisplay();

        if(moveCount <= 0 || !grid.CheckPossibleMatches(grid.xDimension))
        {
            Debug.Log("xDimension (MoveCounter.cs): "+ grid.xDimension.ToString());
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
        moveCountDisplay.enabled = false;

        stageCompletedDisplay.SetActive(true);
        int currentLevel = GetCurrentLevel();
        if(currentLevel >= PlayerPrefs.GetInt("levelReached", 1)) {
            PlayerPrefs.SetInt("levelReached", currentLevel + 1);
        }

        // Save level progress after each stage completion
        PlayerPrefs.Save();
    }

    private int GetCurrentLevel()
    {
        return int.Parse(SceneManager.GetActiveScene().name.Replace("Level", ""));
    }

}

