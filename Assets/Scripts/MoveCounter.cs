using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveCounter : MonoBehaviour
{
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

        if(moveCount <= 0)
        {
            StageCompleted();
        }
    }

    private void UpdateMoveCountDisplay()
    {
        moveCountDisplay.text = "Moves Left: " + moveCount;
    }

    private void StageCompleted()
    {
        isGameOver = true;
        moveCountDisplay.enabled = false;
        stageCompletedDisplay.SetActive(true);
    }
}
