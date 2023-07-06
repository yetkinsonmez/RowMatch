using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;


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
        moveCount = PlayerPrefs.GetInt("MoveCount", 10);
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

    public void StageCompleted()
    {
        isGameOver = true;

        StartCoroutine(ShowStageCompletedDisplayAfterDelay(0.75f));        
    }


    private IEnumerator ShowStageCompletedDisplayAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay

        stageCompletedDisplay.SetActive(true);
    }


}

