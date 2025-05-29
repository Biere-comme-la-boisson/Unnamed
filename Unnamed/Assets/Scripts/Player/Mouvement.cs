using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouvement : MonoBehaviour
{
    [Header("Player Components")]
    public CharacterController player;
    public Transform groundCheck;

    [Header("Player Settings")]
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    [Header("Player State")]
    private Vector3 velocity;

    void Start()
    {
        
    }

    void Update()
    {
        Sprint();
        Movement();
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        player.Move(move * speed * Time.deltaTime);

        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, LayerMask.GetMask("Ground"));
        if (player.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        velocity.y += gravity * Time.deltaTime;
        player.Move(velocity * Time.deltaTime);
        if (Input.GetButtonDown("Jump") && player.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
    }

    void Sprint()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 10f;
        }
        else
        {
            speed = 5f;
        }
    }
}
