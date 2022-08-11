using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using System.Linq;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public GameObject titleMenu, selectMenu;
    public static bool freshStart = true;
    public static List<string> gamesList;
    public TextMeshProUGUI[] buttonText;

    public void Start()
    {
        titleMenu.SetActive(freshStart);
        selectMenu.SetActive(!freshStart);
        freshStart = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            titleMenu.SetActive(true);
            selectMenu.SetActive(false);
        }
    }
    public void ToggleSelect()
    {
        titleMenu.SetActive(!titleMenu.activeSelf);
        selectMenu.SetActive(!selectMenu.activeSelf);
    }
    public void SelectGame(TextMeshProUGUI game)
    {
        SceneManager.LoadScene(game.text);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
