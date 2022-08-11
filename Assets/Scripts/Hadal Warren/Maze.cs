using System.Collections.Generic;
using UnityEngine;

public class Maze
{
    public GameObject maze = new GameObject("Maze");
    public MazeBlock[,] mazeBlocksGrid, groundBlocksGrid;
    public List<List<MazeBlock>> temporaryNodeLists = new List<List<MazeBlock>>();
    public List<MazeBlock> nodeList = new List<MazeBlock>();
    public List<MazeBlock> permanentWallList = new List<MazeBlock>();
    public List<MazeBlock> initialWallList = new List<MazeBlock>();
    public List<MazeBlock> droppableWallList = new List<MazeBlock>();
    public List<MazeBlock> groundBlocksList = new List<MazeBlock>();
    public int sizeX, sizeY, scale;

    /// <summary> 
    /// Generate maze of size x by y at given s scale, with d extra walls dropped,
    /// with walls using wM material, and with ground using gM material.
    /// (Wall sections take the same amount of space as path sections.)
    /// (Sections are represented by Unity primitive cube.)
    /// (Maze of size x by y will take up x*2+1 by y*2+1 space.)
    /// </summary> 
    public Maze(int x, int y, int s, int d, Material wM, Material gM)
    {
        
        sizeX = x;
        sizeY = y;
        scale = s;

        if (x == 0)
            sizeX = 2;
        if (y == 0)
            sizeY = 2;

        mazeBlocksGrid = new MazeBlock[sizeX * 2 + 1, sizeY * 2 + 1];
        groundBlocksGrid = new MazeBlock[sizeX * 2 + 1, sizeY * 2 + 1];
        
        SpawnBlocks(sizeX * 2 + 1, sizeY * 2 + 1);
        
        GenerateMaze();

        DropRandomWalls(d);

        SetNodeNeighbours();

        SetMaterials(wM, gM);
        
        maze.transform.localScale = new Vector3(scale, scale, scale);

        maze.AddComponent<Rigidbody>();
        maze.GetComponent<Rigidbody>().isKinematic = true;
    }

    ~Maze()
    {
        Debug.Log("New maze!");
    }

    /// <summary> Create maze blocks, without connecting maze nodes </summary> 
    void SpawnBlocks(int x, int y)
    {
        int k = 0;
        bool initialWall = false;

        // Initial creation and placement of maze blocks
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                mazeBlocksGrid[i, j] = new MazeBlock(i, j);
                mazeBlocksGrid[i, j].block.transform.position = new Vector3(i, 0, j);
                mazeBlocksGrid[i, j].block.transform.SetParent(maze.transform);
            }
        }

        // Categorize maze blocks
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                // maze block is on the perimeter of the maze, or is a permanent internal wall block
                if (i == 0 || i == x-1 || j == 0 || j == y - 1 || (i % 2 == 0 && j % 2 == 0))
                {
                    // Raise blocks to show them
                    //mazeBlocksGrid[i, j].block.transform.position += new Vector3(0, 2, 0);
                    permanentWallList.Add(mazeBlocksGrid[i, j]);
                } 
                // maze block is an internal wall block that may be dropped to create a maze pathway
                else if (initialWall)
                {
                    // Raise blocks to show them
                    //mazeBlocksGrid[i, j].block.transform.position += new Vector3(0, 5, 0);
                    
                    // assign neighbour maze blocks to maze block for later reference
                    if (i % 2 == 1)
                    {
                        mazeBlocksGrid[i, j].neighbours.Add(mazeBlocksGrid[i, j - 1]);
                        mazeBlocksGrid[i, j].neighbours.Add(mazeBlocksGrid[i, j + 1]);
                        //Debug.Log(i.ToString() + "," + (j-1).ToString());
                        //Debug.Log(i.ToString() + "," + (j + 1).ToString());
                    }
                    else
                    {
                        mazeBlocksGrid[i, j].neighbours.Add(mazeBlocksGrid[i - 1, j]);
                        mazeBlocksGrid[i, j].neighbours.Add(mazeBlocksGrid[i + 1, j]);
                        //Debug.Log((i-1).ToString() + "," + j.ToString());
                        //Debug.Log((i+1).ToString() + "," + j.ToString());
                    }
                    initialWallList.Add(mazeBlocksGrid[i, j]);
                }
                // maze block is a permanent ground block
                else if (i % 2 == 1 && j % 2 == 1) 
                {
                    // Raise blocks to show them
                    // mazeBlocksGrid[i, j].block.transform.position += new Vector3(0, 5, 0);
                    mazeBlocksGrid[i, j].block.transform.position += new Vector3(0, -1, 0);
                    groundBlocksGrid[i, j] = mazeBlocksGrid[i, j];
                    groundBlocksList.Add(mazeBlocksGrid[i, j]);
                    temporaryNodeLists.Add(new List<MazeBlock>() { mazeBlocksGrid[i, j] });
                    nodeList.Add(mazeBlocksGrid[i, j]);
                    mazeBlocksGrid[i, j].group = k;
                    k++;
                }
                initialWall = !initialWall;          
            }
        }
    }

    // Make use of a randomized Kruskal's algorithm to connect maze nodes
    void GenerateMaze()
    {
        // While there are viable walls to drop down
        while (initialWallList.Count > 0)
        {
            // Pick random wall to consider dropping down
            int dropIndex = Random.Range(0, initialWallList.Count);
            // See what group(s) the nodes adjacent to the wall are in.
            int group1 = initialWallList[dropIndex].neighbours[0].group;
            int group2 = initialWallList[dropIndex].neighbours[1].group;
            // Drop wall if adjacent nodes are not in the same group.
            if (group1 != group2)
            {
                initialWallList[dropIndex].block.transform.position += new Vector3(0, -1, 0);
                // Consider dropped wall block as a ground block
                groundBlocksGrid[initialWallList[dropIndex].xCord, initialWallList[dropIndex].zCord] = initialWallList[dropIndex];
                groundBlocksList.Add(initialWallList[dropIndex]);
                // While the second group has at least 1 node
                while (temporaryNodeLists[group2].Count > 0)
                {
                    // Move the first node from second group into the first group
                    temporaryNodeLists[group2][0].group = group1;
                    temporaryNodeLists[group1].Add(temporaryNodeLists[group2][0]);
                    temporaryNodeLists[group2].Remove(temporaryNodeLists[group2][0]);
                }
            }
            else
            {
                //consider wall for dropping later
                droppableWallList.Add(initialWallList[dropIndex]);
            }
            // Remove wall from list of walls to initially consider dropping
            initialWallList.RemoveAt(dropIndex);
        }
    }

    void DropRandomWalls(int amount)
    {
        // if there are droppable walls
        if (droppableWallList.Count > 0)
        {
            // if the function is asked to drop more walls than there actually are, treat as if asked to drop all walls
            if (amount > droppableWallList.Count)
                amount = droppableWallList.Count;

            // drop random walls until the amount of walls to drop has been met
            for (int i = 0; i < amount; i++)
            {
                int dropIndex = Random.Range(0, droppableWallList.Count);

                droppableWallList[dropIndex].block.transform.position += new Vector3(0, -1, 0);

                // Consider dropped wall block as a ground block
                groundBlocksGrid[droppableWallList[dropIndex].xCord, droppableWallList[dropIndex].zCord] = droppableWallList[dropIndex];
                groundBlocksList.Add(droppableWallList[dropIndex]);

                droppableWallList.RemoveAt(dropIndex);
            }
        }
        else
        {
            Debug.Log("No droppable walls.");
        }
    }

    // for each ground block, save a reference for each of its neighbours
    void SetNodeNeighbours()
    {
        for (int i = 0; i < nodeList.Count; i++)
        {
            if (groundBlocksGrid[nodeList[i].xCord, nodeList[i].zCord + 1]!=null)
            {
                nodeList[i].neighbours.Add(groundBlocksGrid[nodeList[i].xCord, nodeList[i].zCord+1]);
            }
            if (groundBlocksGrid[nodeList[i].xCord + 1, nodeList[i].zCord]!=null)
            {
                nodeList[i].neighbours.Add(groundBlocksGrid[nodeList[i].xCord+1, nodeList[i].zCord]);
            }
            if (groundBlocksGrid[nodeList[i].xCord, nodeList[i].zCord - 1]!=null)
            {
                nodeList[i].neighbours.Add(groundBlocksGrid[nodeList[i].xCord, nodeList[i].zCord - 1]);
            }
            if (groundBlocksGrid[nodeList[i].xCord - 1, nodeList[i].zCord]!=null)
            {
                nodeList[i].neighbours.Add(groundBlocksGrid[nodeList[i].xCord - 1, nodeList[i].zCord]);
            }
        }
    }

    // set the material of both the wall maze blocks and the ground maze blocks
    void SetMaterials(Material wallMaterial, Material groundMaterial)
    {
        for (int i = 0; i < permanentWallList.Count; i++)
        {
            permanentWallList[i].block.GetComponent<Renderer>().material = wallMaterial;
        }

        for (int i = 0; i < droppableWallList.Count; i++)
        {
            droppableWallList[i].block.GetComponent<Renderer>().material = wallMaterial;
        }

        for (int i = 0; i < groundBlocksList.Count; i++)
        {
            groundBlocksList[i].block.GetComponent<Renderer>().material = groundMaterial;
        }
    }
}

public class MazeBlock
{
    public GameObject block = GameObject.CreatePrimitive(PrimitiveType.Cube);
    public int xCord, zCord, group; 
    public List<MazeBlock> neighbours = new List<MazeBlock>();

    public MazeBlock(int x, int z)
    {
        xCord = x;
        zCord = z;
    }
}