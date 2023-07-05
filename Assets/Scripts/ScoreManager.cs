using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScoreManager : MonoBehaviour
{
    public int redScore = 100;
    public int greenScore = 150;
    public int blueScore = 200;
    public int yellowScore = 250;
    
    private int currentScore = 0;

    public Text scoreDisplay;

    public void Start()
    {
        UpdateScoreDisplay();
    }

    public void AddScore(ColoredItem.ColorType colorType)
    {
        switch (colorType)
        {
            case ColoredItem.ColorType.Red:
                currentScore += redScore;
                break;
            case ColoredItem.ColorType.Green:
                currentScore += greenScore;
                break;
            case ColoredItem.ColorType.Blue:
                currentScore += blueScore;
                break;
            case ColoredItem.ColorType.Yellow:
                currentScore += yellowScore;
                break;
            default:
                break;
        }
        
        UpdateScoreDisplay();
    }

    private void UpdateScoreDisplay()
    {
        scoreDisplay.text = "Score\n" + currentScore;

        scoreDisplay.transform.DOScale(0.45f, 0.1f).OnComplete(() => 
        {
            scoreDisplay.transform.DOScale(0.3f, 0.1f);
        });

        scoreDisplay.DOColor(Color.yellow, 0.1f).OnComplete(() => 
        {
            scoreDisplay.DOColor(Color.white, 0.1f);
        });
    }

    public int GetScore()
    {
        return currentScore;
    }
}
