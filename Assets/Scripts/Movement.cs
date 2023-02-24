using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Movement : MonoBehaviour
{
    [Header("Animation")]
    private Animator animator;
    private string currentState;
    [Space]
    public string cootsIdle;
    public string cootsWalk;
    public string cootsJump;

    [Header("Movement")]
    public HealthBar healthBar;
    private Rigidbody2D rb;
    public Vector2 moveInput;
    private Vector2 startPos;
    public bool facingRight;
    public float runSpeed;
    public float topSpeed;
    [Space]
    private AudioSource audioSource;
    public AudioClip jumpSound;
    public AudioClip rewardSound;
    public float jumpPitch;
    public float jumpSoundVolume;
    public float rewardPitch;
    public float rewardSoundVolume;
    [Space]
    public TMP_Text killCountText;
    public GameObject deadScreen;
    public float goobKillCount;
    public float maxHealth;
    public float healAmount;
    private float currentHealth;

    [Header("Jumping")]
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float coyoteTime;
    public float coyoteTimeCounter;
    public float jumpBuffer;
    public float jumpBufferCounter;
    public float gravityScale;
    public float fallMultiplier;
    public float jumpForce;
    private bool isGrounded;

    void Start()
    {
        Time.timeScale = 1;

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        rb.gravityScale = gravityScale;
        startPos = transform.position;
    }

    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer);
        
        StateHandler();
        healthBar.SetHealth(currentHealth);
        
        killCountText.text = $"Goobs Killed: {goobKillCount}";

        if (currentHealth <= 0) {
            deadScreen.SetActive(true);
            Time.timeScale = 0;
        }

        coyoteTimeCounter -= Time.deltaTime;

        if (isGrounded) {
            coyoteTimeCounter = coyoteTime;
        }

        if (moveInput.x != 0) {
			CheckDirectionToFace(moveInput.x > 0);
        }

        if (rb.velocity.y < 0f) {
            rb.gravityScale = gravityScale * fallMultiplier;
        }

        else {
            rb.gravityScale = gravityScale;
        }

        jumpBufferCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Space)) {
            jumpBufferCounter = jumpBuffer;
        }

        if ((jumpBufferCounter > 0f) && (coyoteTimeCounter > 0f)) {
            jumpBufferCounter = 0;
            coyoteTimeCounter = 0;

            audioSource.PlayOneShot(jumpSound, jumpSoundVolume);
            audioSource.pitch = jumpPitch;

            Jump();
        }
    }

    void FixedUpdate() {
        if (moveInput != Vector2.zero) {
            rb.AddForce(new Vector2(moveInput.x * runSpeed, 0), ForceMode2D.Impulse);
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -topSpeed, topSpeed), rb.velocity.y);
        }

        else {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, runSpeed * Time.deltaTime), rb.velocity.y);
        }
    }

    void Flip() {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        facingRight = !facingRight;
    }

    void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != facingRight)
			Flip();
	}

    void Jump() {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    void StateHandler() {
        if (isGrounded) {
            if (moveInput.x != 0) {
                ChangeAnimationState(cootsWalk);
            }

            else {
                ChangeAnimationState(cootsIdle);
            }
        }

        else if ((jumpBufferCounter > 0f)) {
            ChangeAnimationState(cootsJump);
        }
        
        else {
            ChangeAnimationState(cootsJump);
        }
    }

    void ChangeAnimationState(string newState) {
        if (currentState == newState) return;

        animator.Play(newState);

        currentState = newState;
    }
    
    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.name == "Water") {
            transform.position = startPos;
            rb.velocity = Vector2.zero;
        }

        if (collider.name.Contains("EnemyTrigger"))
        {
            if (rb.velocity.y < 0f) {
                collider.transform.parent.GetComponent<Goob>().isHit = true;
                
                collider.transform.parent.GetComponent<Goob>().audioSource.PlayOneShot(collider.transform.parent.GetComponent<Goob>().hitSound, collider.transform.parent.GetComponent<Goob>().hitSoundVolume);
                collider.transform.parent.GetComponent<Goob>().audioSource.pitch = collider.transform.parent.GetComponent<Goob>().hitSoundPitch;

                audioSource.PlayOneShot(rewardSound, rewardSoundVolume);
                audioSource.pitch = rewardPitch;
                goobKillCount++;
                
                Heal(healAmount);
                Jump();
            }
        }
    }

    public void Damage(float damage) {
        currentHealth -= damage;
    }

    public void Heal(float heal) {
        if (currentHealth < maxHealth) {
            currentHealth += heal;
        }
    }
}
