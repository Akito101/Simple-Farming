using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    private Rigidbody2D rb;
    private float x;
    private float y;
    private Vector2 input;
    public Animator anim;
    private bool moving;

    // Pickaxe
    public GameObject pickaxe;
    private bool isSwinging = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (pickaxe != null)
        {
            pickaxe.SetActive(false);
        }
    }

    void Update()
    {
        GetInput();

        if (Input.GetKeyDown(KeyCode.F) && !isSwinging)
        {
            StartCoroutine(SwingPickaxe());
        }


        Animate();
    }

    void FixedUpdate()
    {
        if (!isSwinging)
        {
            rb.linearVelocity = input * movementSpeed;
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void GetInput()
    {
        if (isSwinging)
        {
            input = Vector2.zero;
            x = 0;
            y = 0;
            return;
        }

        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");

        input = new Vector2(x, y).normalized;
    }

    void Animate()
    {
        if (isSwinging)
        {
            moving = false;

            // Force player to face front
            anim.SetFloat("X", 0);
            anim.SetFloat("Y", -1);

            anim.SetBool("Moving", false);
            return;
        }

        if (input.magnitude > 0.1f)
        {
            moving = true;
        }
        else
        {
            moving = false;
        }

        if (moving)
        {
            anim.SetFloat("X", x);
            anim.SetFloat("Y", y);
        }

        anim.SetBool("Moving", moving);
    }

    IEnumerator SwingPickaxe()
    {
        isSwinging = true;

        // Force front idle immediately
        anim.SetFloat("X", 0);
        anim.SetFloat("Y", -1);
        anim.SetBool("Moving", false);

        if (pickaxe != null)
        {
            pickaxe.SetActive(true);

            Transform axe = pickaxe.transform;
            Quaternion startRot = axe.localRotation;
            Quaternion swingRot = Quaternion.Euler(0, 0, 40f);

            float time = 0f;

            while (time < 0.12f)
            {
                axe.localRotation = Quaternion.Lerp(startRot, swingRot, time / 0.12f);
                time += Time.deltaTime;
                yield return null;
            }

            axe.localRotation = swingRot;

            yield return new WaitForSeconds(0.08f);

            time = 0f;

            while (time < 0.12f)
            {
                axe.localRotation = Quaternion.Lerp(swingRot, startRot, time / 0.12f);
                time += Time.deltaTime;
                yield return null;
            }

            axe.localRotation = startRot;
            pickaxe.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(0.25f);
        }

        isSwinging = false;
    }
}