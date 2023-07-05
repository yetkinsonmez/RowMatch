
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class LevelsPopup : MonoBehaviour
{
    public GameObject LevelButtonPrefab;
    public Transform LevelsContainer;
    private int levelCount;
    public int[] initialLevelMoveCounts;
    public Button cancelButton; 

    public MainMenu mainMenu;


    private void Awake()
    {
        cancelButton.onClick.AddListener(Cancel);
    }

    private void OnEnable()
    {

        levelCount = mainMenu.urls.Length;
        
        // populate the levels
        PopulateLevels();

        // slide-in animation for the panel
        transform.localPosition = new Vector3(0, -Screen.height, 0);
        transform.DOLocalMoveY(-20, 1.2f).SetEase(Ease.OutCubic);

        // sequential appearance for the level buttons
        foreach (Transform child in LevelsContainer)
        {
            child.localScale = Vector3.zero; // make each button invisible
            Sequence mySequence = DOTween.Sequence();
            mySequence.AppendInterval(0.1f * child.GetSiblingIndex()); // delay based on button order
            mySequence.Append(child.DOScale(1f, 0.7f)); // make each button appear gradually
            mySequence.Play();
        }
    }


    private void PopulateLevels()
    {
        // Clean up before populating
        foreach (Transform child in LevelsContainer)
        {
            Destroy(child.gameObject);
        }

        // get the highest level reached
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        // Create button for each level
        for (int i = 1; i <= levelCount; i++)
        {
            GameObject levelButton = Instantiate(LevelButtonPrefab, LevelsContainer);
            
            // levelButton.transform.localScale = new Vector3(0.8f, 0.7f, 1f);

            // Here we fetch and display highest score and move count
            string levelStats = PlayerPrefs.GetString("LevelStats" + i, "0,0"); // Format is "HighestScore,MoveCount"
            
            string[] stats = levelStats.Split(',');

            Text levelText = levelButton.transform.Find("LevelNo").GetComponent<Text>();
            Text statsText = levelButton.transform.Find("LevelDetail").GetComponent<Text>();

            levelText.text = "Level " + i;

            int initialLevelMoveCount = initialLevelMoveCounts[i-1];
            statsText.text = "Highest Score: " + stats[0] + " | Moves: " + initialLevelMoveCount;


            // disable the button if the level is not reached yet
            Button button = levelButton.GetComponent<Button>();
            if(i > levelReached) {
                button.interactable = false;
            }

            int levelIndex = i; // Important to capture in local variable for delegate
            button.onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    private void LoadLevel(int levelIndex)
    {
        string url = "https://row-match.s3.amazonaws.com/levels/RM_A" + levelIndex;
        string jsonLevelData = PlayerPrefs.GetString(url);
        MainMenu.LevelData levelData = JsonUtility.FromJson<MainMenu.LevelData>(jsonLevelData);

        PlayerPrefs.SetInt("GridWidth", levelData.GridWidth);
        PlayerPrefs.SetInt("GridHeight", levelData.GridHeight);
        PlayerPrefs.SetInt("MoveCount", levelData.MoveCount);
        SceneManager.LoadScene("Game");
    }


    private void Cancel() // add this Cancel function
    {
        transform.DOLocalMoveY(-Screen.height, 0.5f).SetEase(Ease.InCubic).OnComplete(() => gameObject.SetActive(false));
    }
}
