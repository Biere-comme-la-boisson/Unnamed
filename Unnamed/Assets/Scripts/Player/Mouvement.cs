using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouvement : MonoBehaviour
{
    [Header("Components")]
    public CharacterController player;
    public Transform groundCheck;

    [Header("Stats")]
    public float speed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public float energy = 100f;
    private float depletedTime = 0f;
    public float depletionTime = 2f;

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
        if (Input.GetKey(KeyCode.LeftShift) && energy > 0f)
        {
            speed = 10f;
            energy -= Time.deltaTime * 10f;
            if (energy < 0f)
                energy = 0f;
            if (energy == 0f)
            {
                depletedTime = Time.time;
            }
        }
        else
        {
            speed = 5f;
            if (energy < 100f && Time.time >= depletionTime + depletedTime)
                energy += Time.deltaTime * 12f;
            if (energy > 100f)
                energy = 100f;
        }
    }
}
