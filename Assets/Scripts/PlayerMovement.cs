using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public float runningSpeed = 400f; // public variables can be modified in unity editor
    private float horizontalMove = 0f;
    private bool jump;
    private bool crouch;
    public Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update() { // Used to get Input from the player
        horizontalMove = Input.GetAxisRaw("Horizontal") * runningSpeed; // Input.GetAxisRaw("Horizontal") returns -1 if left arrow key or A is pressed, 0 if no key is pressed, 1 if right arrow key or D is pressed
        animator.SetFloat("horizontalSpeed", Mathf.Abs(horizontalMove));

        if (Input.GetButton("Jump")) {
            jump = true;
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButton("Crouch")) {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch")) {
            crouch = false;
        }

        if (Input.GetButton("Look Up")) {
            animator.SetBool("isLookingUp", true);
        } else if (Input.GetButtonUp("Look Up")) {
            animator.SetBool("isLookingUp", false);
        }
    }

    // Used for physics-related updates
    void FixedUpdate() { // Used to apply the input to the player
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump); // Time.fixedDeltaTime allows us to move the player at a constant speed
        jump = false;
    }

    public void OnLanding() {
        animator.SetBool("isJumping", false);
    }

    public void OnCrouch(bool isCrouching) {
        animator.SetBool("isCrouching", isCrouching);
    }
}
