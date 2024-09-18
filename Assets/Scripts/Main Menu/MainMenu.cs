using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionMenu;

    private void Awake()
    {
        Time.timeScale = 1f;
    }
    public void GotoMainMenu()
    {
        optionMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void GotoOptionMenu()
    {
        optionMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }
}
