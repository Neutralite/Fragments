using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private CharacterController controller;

    [SerializeField]
    private float speed = 12f,
                  jumpHeight = 3f,
                  gravity = -19.62f,
                  groundDistance = 0.4f;

    private Vector3 velocity;

    [SerializeField]
    private Transform groundChecker;

    [SerializeField]
    private LayerMask groundMask;

    [SerializeField]
    private bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale == 1)
        {
            
            isGrounded = Physics.CheckSphere(groundChecker.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = gravity;
            }

            // horizontal movement 
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;

            controller.Move(move * speed * Time.deltaTime);

            // jumping
            if (Input.GetButton("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
            }

            // gravity
            velocity.y += gravity * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);
        }
    }
}
