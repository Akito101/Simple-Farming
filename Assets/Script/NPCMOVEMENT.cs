using UnityEngine;
using System.Collections;

public class NPCWander : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float wanderTime = 1.5f;
    public float pauseTime = 2f;

    private Rigidbody2D rb;
    private Animator anim;
    private Vector2 moveDirection;
    private bool isMoving;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        StartCoroutine(WanderRoutine());
    }

    IEnumerator WanderRoutine()
    {
        while (true)
        {
            // Choose a random direction (Up, Down, Left, Right, or Stay)
            int choice = Random.Range(0, 5);
            switch (choice)
            {
                case 0: moveDirection = Vector2.up; break;
                case 1: moveDirection = Vector2.down; break;
                case 2: moveDirection = Vector2.left; break;
                case 3: moveDirection = Vector2.right; break;
                case 4: moveDirection = Vector2.zero; break;
            }

            if (moveDirection != Vector2.zero)
            {
                // Update Animator
                anim.SetFloat("MoveX", moveDirection.x);
                anim.SetFloat("MoveY", moveDirection.y);
                anim.SetBool("IsMoving", true);

                yield return new WaitForSeconds(wanderTime);
            }
            else
            {
                anim.SetBool("IsMoving", false);
                yield return new WaitForSeconds(pauseTime);
            }

            // Stop and Wait
            moveDirection = Vector2.zero;
            anim.SetBool("IsMoving", false);
            rb.linearVelocity = Vector2.zero;
            yield return new WaitForSeconds(pauseTime);
        }
    }

    void FixedUpdate()
    {
        if (moveDirection != Vector2.zero)
        {
            rb.linearVelocity = moveDirection * moveSpeed;
        }
    }
}