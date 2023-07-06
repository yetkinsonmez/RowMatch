using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenController : MonoBehaviour
{
    public string gameSceneName = "Game"; // The name of your game scene
    public float delay = 3f; // The delay before loading the game scene

    public AudioSource audioSource;
    public AudioClip infoSound;

    private void Start()
    {
        StartCoroutine(LoadGameSceneAfterDelay());
    }

    private IEnumerator LoadGameSceneAfterDelay()
    {   
        audioSource.PlayOneShot(infoSound);
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(gameSceneName);
    }
}
