using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 1f;

    // Load next level when button is clicked
    public void OnClick()
    {
        LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        // load the next scene numerically
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int LevelIndex)
    {
        // Button Click animation
        transition.SetTrigger("Clicked");
        yield return new WaitForSeconds(transitionTime);

        // Background drop animation
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);

        // load next scene
        SceneManager.LoadScene(LevelIndex);
    }
}
