using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    private Transform referenceTransform;
    
    [SerializeField]
    private GameObject originalCopy;

    [SerializeField]
    private List<GameObject> inactiveClones, activeClones;

    [SerializeField]
    private int maxActiveClones = 10;

    private float timer;

    [SerializeField]
    private float spawnDelay = 1f, 
        _xMinPos, _xMaxPos, 
        _yMinPos, _yMaxPos,
        _zMinPos, _zMaxPos;

    // Start is called before the first frame update
    void Start()
    {
        if (referenceTransform == null)
        {
            referenceTransform = transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            timer += Time.deltaTime;

            if (timer >= spawnDelay)
            {
                if (activeClones.Count < maxActiveClones)
                {
                    if(inactiveClones.Count > 0)
                    {
                        ActivateCopy();
                    }
                    else
                    {
                        SpawnCopy();
                    }
                }
                timer -= spawnDelay;
            }
        }
    }

    private void ActivateCopy()
    {
        SetSpawnPos(inactiveClones[0]);
        inactiveClones[0].SetActive(true);
        MyToolbox.MoveItemBetweenLists(inactiveClones, activeClones, inactiveClones[0]);
    }

    private void SpawnCopy()
    {
        GameObject newClone = Instantiate(originalCopy, transform);
        SetSpawnPos(newClone);
        activeClones.Add(newClone);
    }

    private void SetSpawnPos(GameObject gameObject)
    {
        Vector3 randomVector = MyToolbox.GetRandomVector3(_xMinPos, _xMaxPos, _yMinPos, _yMaxPos, _zMinPos, _zMaxPos);
        gameObject.transform.localPosition = randomVector;
    }

    public void DeactivateCopy(GameObject gameObject)
    {
        MyToolbox.MoveItemBetweenLists(activeClones, inactiveClones, gameObject);
    }
}
