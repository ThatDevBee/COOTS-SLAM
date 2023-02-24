using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    private Animator animator;
    private AudioSource audioSource;
    private string currentState;
    public float launchForce;
    public bool isStretched;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }
    
    void Update() {
        if (isStretched) {
            ChangeAnimationState("BouncePad_Stretch");
        }

        else {
            ChangeAnimationState("BouncePad_Idle");
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.rigidbody != null) {
            isStretched = true;
            audioSource.Play();
            collision.rigidbody.velocity = new Vector2(collision.rigidbody.velocity.x, launchForce);
        }
    }

    void ChangeAnimationState(string newState) {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    public void StopAnimation() {
        isStretched = false;
    }
}
