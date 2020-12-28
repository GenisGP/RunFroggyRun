using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu, optionsMenu, restartWarning, pauseButton;

    public void Pause()
    {
        PlayerManager.gamePaused = true;
        
        pauseMenu.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;

        PlayerManager.gamePaused = false;
    }

    public void Restart()
    {
        pauseMenu.SetActive(false);
        restartWarning.SetActive(true);
    }
    public void AcceptRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void DeclineRestart()
    {
        pauseMenu.SetActive(true);
        restartWarning.SetActive(false);
    }

    public void EnterOptions()
    {
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
    public void ExitOptions()
    {
        pauseMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
