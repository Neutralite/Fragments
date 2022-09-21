using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatekeeperGameManager : GameManagerBase
{
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPausePressed();
    }
}
