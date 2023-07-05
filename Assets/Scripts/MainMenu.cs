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

    // public Dictionary<string, LevelData> levelDataDict;

    public  string[] urls = {
        "https://row-match.s3.amazonaws.com/levels/RM_A1",
        "https://row-match.s3.amazonaws.com/levels/RM_A2"
    };

    private void Start()
    {   
        // DontDestroyOnLoad(gameObject); // keep the levelDataDict
        // levelDataDict = new Dictionary<string, LevelData>(); 

        GetComponent<Button>().onClick.AddListener(OpenLevelsPopup);
        downloadButton.onClick.AddListener(DownloadAndLogContent);
    }

    private void OpenLevelsPopup()
    {
        LevelsPopup.SetActive(true);
    }

    private void DownloadAndLogContent()
    {
        foreach (var url in urls)
        {
            StartCoroutine(DownloadAndLog(url));
        }
    }

    private IEnumerator DownloadAndLog(string url)
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

                // levelDataDict[url] = levelData;


            }
        }
    }
}
