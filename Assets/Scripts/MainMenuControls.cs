using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuControls : MonoBehaviour
{
    public void PlaySceneHealth() {
        SceneManager.LoadScene(1);
    }

    public void PlaySceneIntruder() {
        SceneManager.LoadScene(2);
    }

    public void PlaySceneSurveillance() {
        SceneManager.LoadScene(3);
    }

    public void QuitGame() {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
