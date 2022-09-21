using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HydrasGame : GameManagerBase
{
    public TextMeshProUGUI compassUI, descendUI;

    public Maze maze;
    public int sizeX = 10,
               sizeY = 10,
               scale = 5,
               extraWallsDropped = 15;
    public MazeBlock exit;
    public Vector3 exitPosition = new Vector3(5, 0, 5);
    public Material wallMaterial, groundMaterial, exitMaterial;

    public List<GameObject> treasures, enemies;

    // Start is called before the first frame update
    void Start()
    {
        healthUIName = "Health";
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        // if the player is above the exit
        if (exit.xCord * 5 - 2.5 < player.transform.position.x &&
            player.transform.position.x < exit.xCord * 5 + 2.5 &&
            exit.zCord * 5 - 2.5 < player.transform.position.z &&
            player.transform.position.z < exit.zCord * 5 + 2.5)
        {
            // show prompt ui
            if (descendUI.enabled == false)
            {
                descendUI.enabled = true;
            }

            // go to next maze level
            if (Input.GetKeyDown(KeyCode.E) && !Paused)
            {
                startingPosition = exitPosition;
                Setup();
            }
        } else
        {
           if (descendUI.enabled == true)
            {
                descendUI.enabled = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            Paused = !Paused;
        }
    }
    void Setup()
    {
        SpawnMaze();
        SpawnPlayer();
        SpawnTreasure();
        SpawnEnemies();
        SpawnExit();
    }
    void SpawnMaze()
    {
        if (maze != null)
        {
            Destroy(maze.maze);
        }

        maze = new Maze(sizeX, sizeY, scale, extraWallsDropped, wallMaterial, groundMaterial);
    }

    void SpawnTreasure()
    {
        List<MazeBlock> potentialTreasure = maze.nodeList;
        for (int i = 0; i < treasures.Count; i++)
        {
            int newTreasure;
            do
            {
                newTreasure = Random.Range(0, potentialTreasure.Count);
                treasures[i].transform.position = potentialTreasure[newTreasure].block.transform.position;
                treasures[i].transform.position += new Vector3(0, scale, 0);
            }
            while (treasures[i].transform.position == startingPosition);
            potentialTreasure.RemoveAt(newTreasure);
        }
    }
    void SpawnEnemies()
    {
        List<MazeBlock> potentialEnemy = maze.nodeList;
        for (int i = 0; i < enemies.Count; i++)
        {
            int newEnemy;
            do
            {
                newEnemy = Random.Range(0, potentialEnemy.Count);
                enemies[i].GetComponent<WanderingMine>().SetStart(potentialEnemy[newEnemy],scale);
            }
            while (enemies[i].transform.position.x > startingPosition.x - 25 &&
                   enemies[i].transform.position.x < startingPosition.x + 25 &&
                   enemies[i].transform.position.z > startingPosition.z - 25 &&
                   enemies[i].transform.position.z < startingPosition.z + 25 );

            potentialEnemy.RemoveAt(newEnemy);
        }
    }
    void SpawnExit()
    {
        if (exit != null)
        {
            exit.block.GetComponent<Renderer>().material = groundMaterial;
            exit = null;
        }

        int newExit = Random.Range(0, maze.nodeList.Count);
        exit = maze.nodeList[newExit];
        exitPosition = new Vector3(maze.nodeList[newExit].xCord * 5, 0, maze.nodeList[newExit].zCord * 5);

        exit.block.GetComponent<Renderer>().material = exitMaterial;
    }
    public void UpdateCompassUI(float rotation)
    {
        rotation += compassUI.rectTransform.eulerAngles.z;

        compassUI.rectTransform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }
}
