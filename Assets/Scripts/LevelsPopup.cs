
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
            levelButton.GetComponentInChildren<Text>().text = "Level " + i;

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
