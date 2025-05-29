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
    public float sprintSpeed = 10f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;
    public bool gravityEnabled = true;

    [Header("Energy System")]
    public float energy = 100f;
    private float depletedTime = 0f;
    public float depletionTime = 2f;

    [Header("Flight Stats")]
    public float flightSpeed = 5f;
    public float flightEnergyCost = 10f;
    private bool isFlying = false;

    [Header("Player State")]
    private Vector3 velocity;

    void Start()
    {
        
    }

    void Update()
    {
        Sprint();
        Movement();
        Flight();
    }

    void Movement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        player.Move(move * speed * Time.deltaTime);

        bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, LayerMask.GetMask("Ground"));
        if (player.isGrounded && velocity.y < 0 && gravityEnabled)
        {
            velocity.y = -2f;
        }
        else if (!gravityEnabled)
        {
            velocity.y = 0f;
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
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
            speed = sprintSpeed;
            energy -= Time.deltaTime * 10f;
            if (energy < 0f)
                energy = 0f;

            depletedTime = Time.time;
        }
        else
        {
            speed = 5;
            if (energy < 100f && Time.time >= depletionTime + depletedTime)
                energy += Time.deltaTime * 12f;
            if (energy > 100f)
                energy = 100f;
        }
    }

    void Flight()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!isFlying && energy > 0f)
            {
                player.Move(Vector3.up * flightSpeed * Time.deltaTime);
                isFlying = true;
                gravityEnabled = false;
                velocity.y = 0f;
            }
            else
            {
                isFlying = false;
                gravityEnabled = true;
                depletedTime = Time.time;
            }
        }

        if (isFlying)
        {
            if (Input.GetKey(KeyCode.E))
            {
                player.Move(Vector3.up * flightSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.Q))
            {
                player.Move(Vector3.down * flightSpeed * Time.deltaTime);
            }

            energy -= Time.deltaTime * 10f;
            if (energy <= 0f)
            {
                energy = 0f;
                isFlying = false;
                gravityEnabled = true;
                depletedTime = Time.time;
            }

            bool isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, LayerMask.GetMask("Ground"));
            if (player.isGrounded)
            {
                isFlying = false;
                gravityEnabled = true;
                depletedTime = Time.time;
            }
        }

        if (!isFlying && energy < 100f && Time.time >= depletedTime + depletionTime)
        {
            energy += Time.deltaTime * 12f;
            if (energy > 100f)
                energy = 100f;
        }
    }
}
