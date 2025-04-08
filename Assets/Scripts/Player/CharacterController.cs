using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CharacterController : MonoBehaviour {
	private Vector2 previousVelocity;
	private Vector2 currentAcceleration;
	[SerializeField] private float m_JumpForce = 230f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = 0.4f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	private bool m_wasCrouching = false;										// Whether or not a player was crouching in the last frame
	[Range(0, 0.3f)] [SerializeField] private float m_MovementSmoothing = 0.05f;// How much to smooth out the movement
	private bool m_FacingRight = true;   										// For determining which way the player is currently facing.
	[SerializeField] private bool m_AirControl = true;							// Whether or not a player can control the character while the character is on the air;

	[SerializeField] private LayerMask m_WhatIsSafeArea;						// A mask determining what is safe area to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[HideInInspector] public bool m_Grounded;             						// Whether or not the player is grounded.
	private const float k_GroundedRadius = 0.2f; 								// Radius of the overlap circle to determine if grounded
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	private const float k_enemyKnockbackForceX = 5f;							// The knockback force applied to the player by the enemy on the x-axis
	private const float k_enemyKnockbackForceY = 3f;							// The knockback force applied to the player by the enemy on the y-axis
	private const float k_lethalAreaKnockBackForce = 600f;						// The knockback force applied to the player by spikes on the y-axis
	[HideInInspector] public float m_knockbackCounter;							// A counter counting down the knockback time
	[HideInInspector] public float k_knockbackLength = 0.3f;					// Knockback duration of the player
	[HideInInspector] public bool m_knockbackedFromRight;						// Whether or not the player is knockbacked from right
	[HideInInspector] public bool m_knockbackedFromUp; 							// Whether or not the player is knockbacked from up
	[HideInInspector] public bool m_isCollidedWithEnemy;						// Whether or not the player is collided with an enemy
	[HideInInspector] public bool m_isCollidedWithLethalArea;					// Whether or not the player is collided with a spike
	[HideInInspector] public bool m_isCollidedWithLightningTrap;				// Whether or not the player is collided with a lightning trap
	
	private Vector2 m_Velocity = Vector2.zero;									// Current velocity of the player
	[SerializeField] private GameObject heart;									// A game object demonstrating the heart icon on the UI
    [SerializeField] private Sprite[] health;									// An array including all displays of the heart icon on the UI
	public int m_lives = 3;														// How many lives left for player to die

	[SerializeField] private Animator animator;									// Animator of the player
	[SerializeField] private AudioManager audioManager;							// AudioManager of the player
	private Rigidbody2D m_rb2d;													// Rigidbody of the player

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;												// An event occuring when the player lands
	public UnityEvent<bool> OnCrouchEvent;										// An event occuring when the player is crouching
	// [System.Serializable]
	// public class BoolEvent : UnityEvent<bool> { }
	// public BoolEvent OnCrouchEvent;

	private void Start() {
		m_rb2d = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null) {
			OnLandEvent = new UnityEvent();
		}

		if (OnCrouchEvent == null) {
			OnCrouchEvent = new UnityEvent<bool>();
		}
	}

    private void FixedUpdate() {
		bool wasGrounded = m_Grounded;
		m_Grounded = false;
		Collider2D[] safeColliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsSafeArea);
		for (int i = 0; i < safeColliders.Length; i++) {
			if (safeColliders[i].gameObject != gameObject && !safeColliders[i].isTrigger) {
				m_Grounded = true;
				if (!wasGrounded) {
					OnLandEvent.Invoke();
				}
			}
		}

		// We figured out if the character is falling down by checking the current acceleration of the character on y-axis
		currentAcceleration = (m_rb2d.linearVelocity - previousVelocity) / Time.fixedDeltaTime;
    	previousVelocity = m_rb2d.linearVelocity;
		if (currentAcceleration.y < -9.8f * 3f) { // Note that gravityScale is equal to 3f when not climbing, so the comparison value should be adjusted accordingly
    		// Character's acceleration is faster than gravity meaning that it is falling down
    		animator.SetBool("isJumpingDown", true);
		} else {
		    animator.SetBool("isJumpingDown", false);
		}
	}

	public void Move(float moveX, float moveY, bool crouch, bool jump, bool lookingUp, bool climb) {
		// Only control the player if the player is grounded or airControl is turned on while the player is not knockbacked and not looking up
		if ((m_Grounded || m_AirControl) && m_knockbackCounter <= 0 && !lookingUp) {
			animator.SetBool("isHurt", false);
			if (crouch) {
				if (!m_wasCrouching) {
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}
				moveX *= m_CrouchSpeed;
				// Disable one of the colliders when crouching (In this case, this colider is the Box Collider of the Player)
				if (m_CrouchDisableCollider != null) {
					m_CrouchDisableCollider.enabled = false;
				}
			} else {
				if (m_wasCrouching) {
					m_wasCrouching = false;
					OnCrouchEvent.Invoke(false);
				}
				// Enable the collider when the player is not crouching 
				if (m_CrouchDisableCollider != null) {
					m_CrouchDisableCollider.enabled = true;
				}
			}

			Vector2 targetVelocityX = new Vector2(moveX, m_rb2d.linearVelocity.y);
			m_rb2d.linearVelocity = Vector2.SmoothDamp(m_rb2d.linearVelocity, targetVelocityX, ref m_Velocity, m_MovementSmoothing);

			if (climb) {
				m_rb2d.gravityScale = 1f;
				Vector2 targetVelocityY = new Vector2(m_rb2d.linearVelocity.x, moveY);
				m_rb2d.linearVelocity = Vector2.SmoothDamp(m_rb2d.linearVelocity, targetVelocityY, ref m_Velocity, m_MovementSmoothing);
			} else {
				m_rb2d.gravityScale = 3f;
			}

			if (m_Grounded && jump) {
				m_Grounded = false;
				m_rb2d.AddForce(new Vector2(0f, m_JumpForce));
				if (!audioManager.SFXSource.isPlaying) { // to play the jump audio only once when the player jumps
					audioManager.PlaySFXOneShot(audioManager.jump);
				}
			}

			if (moveX < 0 && m_FacingRight) {
				FlipThePlayer();
			} else if (moveX > 0 && !m_FacingRight) {
				FlipThePlayer();
			}
		} else if (m_knockbackCounter > 0) {
			animator.SetBool("isHurt", true);
			if (m_isCollidedWithLethalArea) {
				if (m_knockbackedFromUp) {
					m_rb2d.AddForce(new Vector2(m_rb2d.linearVelocity.x, -(1/3)*k_lethalAreaKnockBackForce));
				} else {
					m_rb2d.AddForce(new Vector2(m_rb2d.linearVelocity.x, k_lethalAreaKnockBackForce));
				}
				m_isCollidedWithLethalArea = false;
			} else if (m_isCollidedWithEnemy) {
				Vector2 targetKnockbackForce;
				if (m_knockbackedFromRight) {
					targetKnockbackForce = new Vector2(-k_enemyKnockbackForceX, k_enemyKnockbackForceY);
				} else {
					targetKnockbackForce = new Vector2(k_enemyKnockbackForceX, k_enemyKnockbackForceY);
				}
				m_rb2d.linearVelocity = Vector2.SmoothDamp(m_rb2d.linearVelocity, targetKnockbackForce, ref m_Velocity, m_MovementSmoothing);
				m_isCollidedWithEnemy = false;
			}
			m_knockbackCounter -= Time.deltaTime;
		}
	}

	private void FlipThePlayer() {
		m_FacingRight = !m_FacingRight;
		// Multiply the player's x local scale by -1 to mirror the player's sprite along the vertical axis.
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}

	public void HealthUpdate() {
		if (m_lives == 3) {
            heart.GetComponent<Image>().sprite = health[0];
        } else if (m_lives == 2) {
            heart.GetComponent<Image>().sprite = health[1];
        } else if (m_lives == 1) {
            heart.GetComponent<Image>().sprite = health[2];
        } else {
            heart.GetComponent<Image>().sprite = health[3];
        }
	}
	
	public IEnumerator ChangeJumpAnimation() {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("isJumpingUp", false);
        animator.SetBool("isJumpingDown", true);
    }

    private void OnDrawGizmos()	{
		if (m_GroundCheck != null) {
			Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
		}
	}
}