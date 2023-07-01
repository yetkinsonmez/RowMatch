using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LevelsPopup;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OpenLevelsPopup);
    }

    private void OpenLevelsPopup()
    {
        LevelsPopup.SetActive(true);
    }
}
