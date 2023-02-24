using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float climbSpeed;
    public float gravityScale;
    private bool isLadder;
    private bool isClimbing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isLadder) {
            if (Input.GetKey(KeyCode.Space)) {
                isClimbing = true;
            }

            else {
                isClimbing = false;
            }
        }
    }

    void FixedUpdate() {
        if (isClimbing) {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, climbSpeed);
        }

        else {
            rb.gravityScale = gravityScale;
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.CompareTag("Ladder")) {
            isLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.CompareTag("Ladder")) {
            isLadder = false;
            isClimbing = false;
        }
    }
}
