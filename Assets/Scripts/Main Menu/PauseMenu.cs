using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private bool gamePaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject confirmationScreen;
    [SerializeField] private GameObject optionMenu;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void ResumeGame()
    {
        ReturnToPauseMenu();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void OptionMenu()
    {
        pauseMenuUI.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void GotoMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenuScene");
    }

    public void ConfirmationScreen()
    {
        pauseMenuUI.SetActive(false);
        confirmationScreen.SetActive(true);
    }
    public void ReturnToPauseMenu()
    {
        pauseMenuUI.SetActive(true);
        optionMenu.SetActive(false);
        confirmationScreen.SetActive(false);
    }
}
