using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsPopup : MonoBehaviour
{
    public GameObject LevelButtonPrefab;
    public Transform LevelsContainer;
    private int levelCount = 2; //update this according to your total levels

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

        // Create button for each level
        for (int i = 1; i <= levelCount; i++)
        {
            GameObject levelButton = Instantiate(LevelButtonPrefab, LevelsContainer);
            levelButton.GetComponentInChildren<Text>().text = "Level " + i;

            int levelIndex = i; // Important to capture in local variable for delegate
            levelButton.GetComponent<Button>().onClick.AddListener(() => LoadLevel(levelIndex));
        }
    }

    private void LoadLevel(int levelIndex)
    {
        SceneManager.LoadScene("Level" + levelIndex);
    }
}
