using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fragment : MonoBehaviour
{
    public float spinSpeed = 0.1f;
    void Start()
    {
        if (Random.value < 0.5)
            spinSpeed *= -1;
    }
    void Update()
    {
        transform.Rotate(0, spinSpeed, 0);
    }
}
