using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

enum PauseMenuState
{
    MainMenuState,
    ConfirmationScreenState,
    OptionMenuState
}

public class PauseMenu : MonoBehaviour
{
    private bool gamePaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject confirmationScreen;
    [SerializeField] private GameObject optionMenu;
    private PauseMenuState? currentState;
    private void Update()
    {
        PauseMenuHotkey();
    }

    private void PauseMenuHotkey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                switch (currentState)
                {
                    case PauseMenuState.MainMenuState:
                        ResumeGame();
                        break;
                    case PauseMenuState.OptionMenuState:
                        ReturnToPauseMenu();
                        break;
                    case PauseMenuState.ConfirmationScreenState:
                        ReturnToPauseMenu();
                        break;
                }
            }
            else
            {
                PauseGame();
            }
        }
    }

    private void PauseGame()
    {
        currentState = PauseMenuState.MainMenuState;
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    public void ResumeGame()
    {
        currentState = null;
        ReturnToPauseMenu();
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    public void OptionMenu()
    {
        currentState = PauseMenuState.OptionMenuState;
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
        currentState = PauseMenuState.OptionMenuState;
        pauseMenuUI.SetActive(false);
        confirmationScreen.SetActive(true);
    }
    public void ReturnToPauseMenu()
    {
        currentState = PauseMenuState.MainMenuState;
        pauseMenuUI.SetActive(true);
        optionMenu.SetActive(false);
        confirmationScreen.SetActive(false);
    }
}
