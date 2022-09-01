using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerBase : MonoBehaviour
{
    public void SpawnPlayer(Vector3 location = default(Vector3), Vector3 rotation = default(Vector3))
    {
        transform.position = location;

        transform.eulerAngles = rotation;
    }
}
