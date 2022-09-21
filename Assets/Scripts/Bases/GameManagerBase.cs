using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Events;

public abstract class GameManagerBase : MonoBehaviour
{
    public TextMeshProUGUI healthUI, scoreUI;
    public GameObject pauseUI; 
    public GameObject player;
    public string healthUIName;
    public Vector3 startingPosition = new Vector3(5, 0, 5);
    public bool paused = true ;
    public bool Paused
    {
        get => paused;
        protected set
        {
            paused = value;
            Time.timeScale = paused ? 0 : 1;
            pauseUI.SetActive(paused);
            OnPauseChanged.Invoke(paused);
        }
    }

    public UnityEvent<bool> OnPauseChanged;

    public void CheckPausePressed() 
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Paused = !Paused;
            
        }
    }
    public void SpawnPlayer()
    {
        player.GetComponent<PlayerBase>().SpawnPlayer(startingPosition);

    }
    public void UpdateHealthUI(int health)
    {
        healthUI.text = $"{healthUIName}: {health}";
    }
    public void UpdateScoreUI(int score)
    {
        scoreUI.text = $"Score: {score}";
    }
    public void PauseGame()
    {
        Paused = !Paused;
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ToggleCursorLock(bool p)
    {
        Cursor.lockState = p ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
