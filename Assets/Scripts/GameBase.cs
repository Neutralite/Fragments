using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public abstract class GameBase : MonoBehaviour
{
    public TextMeshProUGUI healthUI, scoreUI;
    public GameObject pauseUI, player;
    public string healthUIName;
    public Vector3 startingPosition = new Vector3(5, 0, 5);
    public bool paused = true;

    public void SpawnPlayer()
    {
        player.GetComponent<PlayerBase>().SpawnPlayer(startingPosition);

    }
    public void UpdateHealthUI(int health)
    {
        healthUI.text = healthUIName + ": " + health.ToString();
    }
    public void UpdateScoreUI(int score)
    {
        scoreUI.text = "Score: " + score.ToString();
    }
    public void PauseGame()
    {
        paused = !paused;
        pauseUI.SetActive(paused);
    }
    public void ExitGame()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
