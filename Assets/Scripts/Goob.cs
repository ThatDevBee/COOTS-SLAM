using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goob : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    public AudioSource audioSource;
    public AudioClip attackSound;
    public AudioClip hitSound;
    public float attackSoundVolume;
    public float attackSoundPitch;
    public float hitSoundVolume;
    public float hitSoundPitch;
    private string currentState;
    public bool flip;
    public bool isHit;

    public Transform player;
    public float runSpeed;
    public float topSpeed;
    public float minDistance;
    public float maxDistance;
    public float attackForce;
    public float damageAmount;
    public bool caughtPlayer;
    private float distance;
    private Vector2 difference;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        difference = new Vector2(player.position.x - transform.position.x, 0);
        distance = Vector2.Distance(transform.position, player.position);

        StateHandler();

        if (distance < maxDistance) {
            caughtPlayer = true;
        }

        Vector3 scale = transform.localScale;

        if (player.position.x > transform.position.x) {
            scale.x = Mathf.Abs(scale.x) * -1 * (flip ? -1 : 1);
        }

        else {
            scale.x = Mathf.Abs(scale.x) * (flip ? -1 : 1);
        }

        transform.localScale = scale;
    }

    void StateHandler() {
        if (isHit) {
            ChangeAnimationState("Goob_Hit");
        }

        else {
            if (caughtPlayer) {
                if (distance < minDistance) {
                    ChangeAnimationState("Goob_Attack");
                    rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
                    rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, runSpeed * Time.deltaTime), rb.velocity.y);
                }

                else  {
                    ChangeAnimationState("Goob_Walk");
                    rb.constraints = RigidbodyConstraints2D.FreezeRotation;
                    rb.AddForce(new Vector2(difference.x * runSpeed, 0), ForceMode2D.Impulse);
                    rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -topSpeed, topSpeed), rb.velocity.y);
                }
            }

            else {
                ChangeAnimationState("Goob_Idle");
            }
        }
    }

    public void Attack() {
        Rigidbody2D playerRigidbody = player.GetComponent<Rigidbody2D>();
        Movement playerMovement = player.GetComponent<Movement>();
        audioSource.PlayOneShot(attackSound, attackSoundVolume);

        playerRigidbody.velocity = new Vector2(0, playerRigidbody.velocity.y);
        playerRigidbody.AddForce(difference.normalized * attackForce, ForceMode2D.Impulse);

        playerMovement.Damage(damageAmount);
    }

    void ChangeAnimationState(string newState) {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }

    public void Hit() {
        Destroy(gameObject);
    }
}
