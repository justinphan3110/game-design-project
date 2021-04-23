using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    // VARIABLES
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;
    // References
    private CharacterController controller;
    private Animator anim;


    public GameObject cam1;
    public GameObject cam2;
    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
        print(anim);
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cam2.SetActive(false);
            cam1.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            cam2.SetActive(true);
            cam1.SetActive(false);
        }
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float moveZ = Input.GetAxis("Vertical");
        float moveX = Input.GetAxis("Horizontal");

        moveDirection = new Vector3(moveX, 0, moveZ);
        moveDirection = transform.TransformDirection(moveDirection);



        if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
        {
            // Walk
            Walk();
        }
        else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
        {
            // Run
            Run();
        }
        else if (moveDirection == Vector3.zero)
        {
            // Idle
            Idle();
        }


        moveDirection *= moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        print(moveSpeed);

        controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }



    private void Idle()
    {
        anim.SetFloat("speed", 0);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("speed", 0.5f);
    }

    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("speed", 1);

    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
    }

    private void Attack()
    {
        anim.SetTrigger("Attack");
    }
}
