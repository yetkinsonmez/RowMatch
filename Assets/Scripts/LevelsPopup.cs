
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsPopup : MonoBehaviour
{
    public GameObject LevelButtonPrefab;
    public Transform LevelsContainer;
    private int levelCount = 5; //update this according to your total levels
    public int[] initialLevelMoveCounts;
    
    private void OnEnable()
    {
        PopulateLevels();
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
        SceneManager.LoadScene("Level" + levelIndex);
    }
}
