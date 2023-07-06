
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
    public Button cancelButton; 

    public MainMenu mainMenu;

    public AudioSource audioSource;
    public AudioClip levelStartSound;



    private void Awake()
    {
        cancelButton.onClick.AddListener(Cancel);
    }

    private void OnEnable()
    {
        if (PlayerPrefs.GetInt("hasDownloaded", 0) == 1)
        {
            levelCount = mainMenu.urls.Length + mainMenu.offlineLevels.Length;
        }
        else
        {
            levelCount = mainMenu.offlineLevels.Length;
        }
        
        // populate the levels
        PopulateLevels();

        transform.localPosition = new Vector3(0, -Screen.height, 0);
        transform.DOLocalMoveY(-20, 1.2f).SetEase(Ease.OutCubic);
    }


    private void PopulateLevels()
    {   
        // in case of new leves are added
        foreach (Transform child in LevelsContainer)
        {
            Destroy(child.gameObject);
        }

        int levelReached = PlayerPrefs.GetInt("highestLevel", 1);


        // level buttons
        for (int i = 1; i <= levelCount; i++)
        {   

            GameObject levelButton = Instantiate(LevelButtonPrefab, LevelsContainer);

            Text levelText = levelButton.transform.Find("LevelNo").GetComponent<Text>();
            Text movesText = levelButton.transform.Find("Moves").GetComponent<Text>();
            Text highestScoreText = levelButton.transform.Find("HighestScore").GetComponent<Text>();
            
            
            levelText.text = "Level " + i;

            int highScore = PlayerPrefs.GetInt("Level" + i + "HighScore", 0);
            Debug.Log("Main: " + highScore.ToString());

            if(highScore > 0)
            {
                highestScoreText.text = "Highest Score: " + highScore;
            }
            else
            {
                highestScoreText.text = "No Score";
            }
            
            if (i <= mainMenu.offlineLevels.Length)
            {
                movesText.text = "Moves: " + mainMenu.offlineLevels[i - 1].MoveCount;
            }
            else{
                string url;
                int onlineLevelIndex;
                if (i <= 25){
                    url = "https://row-match.s3.amazonaws.com/levels/RM_A" + (i - 10);
                    onlineLevelIndex = i - 10;
                }
                else // it is RM_B*
                {
                    url = "https://row-match.s3.amazonaws.com/levels/RM_B" + (i - 25);
                    onlineLevelIndex = i - 25;
                }
                string jsonLevelData = PlayerPrefs.GetString(url);
                MainMenu.LevelData levelData = JsonUtility.FromJson<MainMenu.LevelData>(jsonLevelData);
                movesText.text = "Moves: " + levelData.MoveCount;
            }

            Button button = levelButton.GetComponent<Button>();
            Image lockIcon = levelButton.transform.Find("Lock").GetComponent<Image>();


            // locked ones
            if(i > levelReached) {
                button.interactable = false;
                lockIcon.gameObject.SetActive(true);
            }

            int levelIndex = i;
            button.onClick.AddListener(() => StartCoroutine(LoadLevel(levelIndex)));
        }
    }

    private IEnumerator LoadLevel(int levelIndex)
    {
        mainMenu.musicAudioSource.Stop();
        MainMenu.LevelData levelData;

        if (levelIndex <= mainMenu.offlineLevels.Length)
        {
            levelData = mainMenu.offlineLevels[levelIndex - 1];
        }
         else if (levelIndex <= 10 + 15) // if it is RM_A*
        {
            string url = "https://row-match.s3.amazonaws.com/levels/RM_A" + (levelIndex - 10);
            string jsonLevelData = PlayerPrefs.GetString(url);
            levelData = JsonUtility.FromJson<MainMenu.LevelData>(jsonLevelData);
        }
        else // it is RM_B*
        {
            string url = "https://row-match.s3.amazonaws.com/levels/RM_B" + (levelIndex - 10 - 15);
            string jsonLevelData = PlayerPrefs.GetString(url);
            levelData = JsonUtility.FromJson<MainMenu.LevelData>(jsonLevelData);
        }
            
        PlayerPrefs.SetInt("GridWidth", levelData.GridWidth);
        PlayerPrefs.SetInt("GridHeight", levelData.GridHeight);
        PlayerPrefs.SetInt("MoveCount", levelData.MoveCount);

        PlayerPrefs.SetInt("CurrentLevel", levelIndex);
        audioSource.PlayOneShot(levelStartSound);

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene("LoadingScreen");
    }



    private void Cancel()
    {
        transform.DOLocalMoveY(-Screen.height, 0.5f).SetEase(Ease.InCubic).OnComplete(() => gameObject.SetActive(false));
    }
}
