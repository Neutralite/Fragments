using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using System.IO;
//using System.Linq;

public class MainMenu : MonoBehaviour
{
    public GameObject titleMenu;
    public MinigameMenu minigameMenu;
    public FragmentManager fragmentManager;
    public List<string> gamesList;
    public static int currentGame = -1, previousGame = 0;
    public static bool freshStart = true;

    void Start()
    {
        fragmentManager.SpawnFragments();
        
        if (!freshStart)
        {
            fragmentManager.PresentFragment(currentGame);
            minigameMenu.UpdateText(currentGame);
        }

        titleMenu.SetActive(freshStart);
        minigameMenu.gameObject.SetActive(!freshStart);
        freshStart = false;
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && currentGame > -1)
        {
            ReturnToTitle();
        }
        else if ((Input.anyKeyDown && !Input.GetMouseButtonDown(0)) || (Input.GetMouseButtonDown(0) && currentGame < 0))
        {
            NextGame();
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(gamesList[currentGame]);
    }

    public void NextGame() 
    {
        if (currentGame == -1)
        {
            currentGame = previousGame;
            titleMenu.SetActive(false);
            minigameMenu.gameObject.SetActive(true);
        } else 
        {
            fragmentManager.ReturnFragment(currentGame);
            currentGame++;
            if (currentGame == gamesList.Count)
                currentGame = 0;
        }
        fragmentManager.PresentFragment(currentGame);
        minigameMenu.UpdateText(currentGame);
    }

    public void PreviousGame() 
    {
        fragmentManager.ReturnFragment(currentGame);
        currentGame--;
        if (currentGame == -1)
            currentGame = gamesList.Count - 1;
        fragmentManager.PresentFragment(currentGame);
        minigameMenu.UpdateText(currentGame);
    }

    public void ReturnToTitle()
    {
        fragmentManager.ReturnFragment(currentGame);
        previousGame = currentGame;
        currentGame = -1;
        titleMenu.SetActive(true);
        minigameMenu.gameObject.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
