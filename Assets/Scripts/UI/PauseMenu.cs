using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("PauseMenu")) {
            pauseMenu.SetActive(!pauseMenu.activeSelf);
        }
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
