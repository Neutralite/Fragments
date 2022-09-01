using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(transform.forward * 0.1f,Space.World);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -1, 0);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(transform.forward * -0.1f, Space.World);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, 1, 0);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Translate(0,0.1f,0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Translate(0, -0.1f, 0);
        }
    }
}
