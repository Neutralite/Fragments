using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingMine : MonoBehaviour
{
    public HydrasGame game;
    public MazeBlock start, destination;
    public Vector3 offset;
    public bool moving = true;
    float speed = 10f;

    public void SetStart(MazeBlock mazeBlock,int scale)
    {
        start = mazeBlock;
        destination = mazeBlock;
        offset = new Vector3(0, scale, 0);
        transform.position = start.block.transform.position + offset;
        moving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving && !game.Paused)
        { 
            if (destination.block.transform.position.x - 0.1 < transform.position.x &&
                transform.position.x < destination.block.transform.position.x + 0.1 &&
                destination.block.transform.position.z - 0.1 < transform.position.z &&
                transform.position.z < destination.block.transform.position.z + 0.1 )
            {
                start = destination;
                transform.position = start.block.transform.position + offset;
            }
            if (destination == start)
            {
                int neighbour = Random.Range(0, start.neighbours.Count);
                destination = start.neighbours[neighbour];
            }

            Vector3 direction = new Vector3(destination.block.transform.position.x - transform.position.x, 0f, destination.block.transform.position.z - transform.position.z).normalized;
            transform.position += speed * direction * Time.deltaTime;
        }
    }
}