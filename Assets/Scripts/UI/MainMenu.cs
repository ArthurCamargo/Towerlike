using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    public void QuitGame() {
        Debug.Log("Quitting");
        Application.Quit();
    }
}
