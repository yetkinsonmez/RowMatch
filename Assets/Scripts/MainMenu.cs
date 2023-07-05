using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MainMenu : MonoBehaviour
{
    public GameObject LevelsPopup;
    public Button downloadButton;

    public struct LevelData
    {
        public int GridWidth;
        public int GridHeight;
        public int MoveCount;

        public string print()
        {
            return $"GridWidth: {GridWidth}, GridHeight: {GridHeight}, MoveCount: {MoveCount}";
        }
    }

    public LevelData[] offlineLevels = new LevelData[10];

    // public Dictionary<string, LevelData> levelDataDict;

    public  string[] urls = {
        "https://row-match.s3.amazonaws.com/levels/RM_A1",
        "https://row-match.s3.amazonaws.com/levels/RM_A2",
        "https://row-match.s3.amazonaws.com/levels/RM_A3",
        "https://row-match.s3.amazonaws.com/levels/RM_A4",
        "https://row-match.s3.amazonaws.com/levels/RM_A5",
        "https://row-match.s3.amazonaws.com/levels/RM_A6",
        "https://row-match.s3.amazonaws.com/levels/RM_A7",
        "https://row-match.s3.amazonaws.com/levels/RM_A8",
        "https://row-match.s3.amazonaws.com/levels/RM_A9",
        "https://row-match.s3.amazonaws.com/levels/RM_A10",
        "https://row-match.s3.amazonaws.com/levels/RM_A11",
        "https://row-match.s3.amazonaws.com/levels/RM_A12",
        "https://row-match.s3.amazonaws.com/levels/RM_A13",
        "https://row-match.s3.amazonaws.com/levels/RM_A14",
        "https://row-match.s3.amazonaws.com/levels/RM_A15",
    };

    private void Start()
    {   
        GetComponent<Button>().onClick.AddListener(OpenLevelsPopup);
        downloadButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("hasDownloaded", 1); // flag for level creation
            DownloadContent();
        });

        if (!PlayerPrefs.HasKey("offlineLevelsGenerated"))
        {
            // Generate the offline levels
            for (int i = 0; i < offlineLevels.Length; i++)
            {
                // This is an example, replace these values with your own level data
                offlineLevels[i] = new LevelData
                {
                    GridWidth = UnityEngine.Random.Range(4, 9),   
                    GridHeight = UnityEngine.Random.Range(4, 11),
                    MoveCount = UnityEngine.Random.Range(8, 17)
                };
                
                // Convert to JSON and save to PlayerPrefs
                string jsonLevelData = JsonUtility.ToJson(offlineLevels[i]);
                PlayerPrefs.SetString("OfflineLevel" + i, jsonLevelData);
            }

            PlayerPrefs.SetInt("offlineLevelsGenerated", 1);
        }
        else
        {
            // Load the offline levels
            for (int i = 0; i < offlineLevels.Length; i++)
            {
                string jsonLevelData = PlayerPrefs.GetString("OfflineLevel" + i);
                offlineLevels[i] = JsonUtility.FromJson<LevelData>(jsonLevelData);
            }
        }
    }


    private void OpenLevelsPopup()
    {
        LevelsPopup.SetActive(true);
    }

    private void DownloadContent()
    {
        foreach (var url in urls)
        {
            StartCoroutine(DownloadLevels(url));
        }
    }

    private IEnumerator DownloadLevels(string url)
    {
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                
                string[] lines = www.downloadHandler.text.Split('\n');
                LevelData levelData = new LevelData
                {
                    GridWidth = int.Parse(lines[1].Split(':')[1]),
                    GridHeight = int.Parse(lines[2].Split(':')[1]),
                    MoveCount = int.Parse(lines[3].Split(':')[1])
                };

                Debug.Log(levelData.print());

                string jsonLevelData = JsonUtility.ToJson(levelData);
                PlayerPrefs.SetString(url, jsonLevelData);

            }
        }
    }
}
