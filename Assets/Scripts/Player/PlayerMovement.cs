using System;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float runningSpeed = 400f; // public variables can be modified in unity editor
    [SerializeField] private float climbingSpeed;
    private float horizontalMove = 0f;
    private float verticalMove = 0f;
    private bool jump;
    private bool crouch;
    private bool lookingUp;
    [HideInInspector] public bool climb;
    public Animator animator;
    [SerializeField] private GameOverScreen gameOverScreen;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private GameObject gameOverScreenUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() { // Used to get Input from the player
        horizontalMove = Input.GetAxisRaw("Horizontal") * runningSpeed; // Input.GetAxisRaw("Horizontal") returns -1 if left arrow key or A is pressed, 0 if no key is pressed, 1 if right arrow key or D is pressed. See Edit -> Project Settings -> Input Manager -> Horizontal, Vertical
        verticalMove = Input.GetAxisRaw("Vertical") * climbingSpeed;
        animator.SetFloat("horizontalSpeed", Mathf.Abs(horizontalMove));
        animator.SetFloat("verticalSpeed", Mathf.Abs(verticalMove));
        if (Mathf.Abs(horizontalMove) != 0 && controller.m_Grounded && !crouch && !climb) {
            audioManager.runningSource.enabled = true;
        } else if (Mathf.Abs(horizontalMove) == 0 || !controller.m_Grounded || crouch || climb) {
            audioManager.runningSource.enabled = false;
        }

        if (Input.GetButton("Crouch")) {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch")) {
            crouch = false;
        }

        if (Input.GetButton("Jump") && controller.m_Grounded) {
            jump = true;
            crouch = false;
            animator.SetBool("isJumpingUp", true);
            animator.SetBool("isJumpingDown", false);
            StartCoroutine(controller.ChangeJumpAnimation());
        }
        if (!controller.m_Grounded) {
            crouch = false;
        }

        if (Input.GetButton("Look Up")) {
            lookingUp = true;
            animator.SetBool("isLookingUp", true);
        } else if (Input.GetButtonUp("Look Up")) {
            lookingUp = false;
            animator.SetBool("isLookingUp", false);
        }

        if (Math.Abs(verticalMove) > 0f && animator.GetBool("isClimbing")) {
            climb = true;
            audioManager.climbingSource.enabled = true;
            crouch = false; // don't let the player crouch while the player is climbing the ladder
        }
        if (Math.Abs(verticalMove) == 0f && animator.GetBool("isClimbing") && !controller.m_Grounded) {
            audioManager.climbingSource.enabled = false;
            crouch = false; // don't let the player crouch while the player is idle on the ladder and is not grounded
        }
        if (Math.Abs(verticalMove) == 0f && animator.GetBool("isClimbing") && controller.m_Grounded) {
            audioManager.climbingSource.enabled = false;
            if (Input.GetButton("Crouch")) {
                crouch = true; // allow the player crouch while the player is idle on the ladder, is grounded and pressed the Crouch button
            }
        }
        if (!animator.GetBool("isClimbing")) {
            audioManager.climbingSource.enabled = false;
        }

        if (crouch) {
            audioManager.crouchingSource.enabled = true;
        } else if (!crouch) {
            audioManager.crouchingSource.enabled = false;
        }
    }

    // Used for physics-related updates
    void FixedUpdate() { // Used to apply the input to the player
        if (controller.m_lives > 0) {
            controller.Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, crouch, jump, lookingUp, climb); // Time.fixedDeltaTime allows us to move the player at a constant speed
            jump = false;
        } else {
            if (!gameOverScreenUI.activeInHierarchy) {
                gameOverScreen.GameOver();
            }
        }
    }

    public void OnLanding() {
        if (animator.GetBool("isJumpingUp")) {
            animator.SetBool("isJumpingUp", false);
        }
        if (animator.GetBool("isJumpingDown")) {
            animator.SetBool("isJumpingDown", false);
        }
    }

    public void OnCrouch(bool isCrouching) {
        animator.SetBool("isCrouching", isCrouching);
    }
}
