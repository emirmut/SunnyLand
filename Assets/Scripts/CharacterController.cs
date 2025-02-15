using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour {
	[SerializeField] private float m_JumpForce = 230f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = 0.4f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	private bool m_wasCrouching = false;										// Whether or not a player was crouching in the last frame
	[Range(0, 0.3f)] [SerializeField] private float m_MovementSmoothing = 0.05f;// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = true;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsSafeArea;						// A mask determining what is safe area to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching
	[SerializeField] public int m_lives = 3;									// How many lives left for player to die 
	private const float k_enemyKnockbackForceX = 5f;
	private const float k_enemyKnockbackForceY = 3f;
	private const float k_lethalAreaKnockBackForce = 10f;
	public float m_knockbackCounter;
	public float k_knockbackLength = 0.3f;
	public bool m_knockbackedFromRight;
	public bool m_isCollidedWithEnemy;
	public bool m_isCollidedWithLethalArea;
	[SerializeField] private Animator animator;

	const float k_GroundedRadius = 0.2f; // Radius of the overlap circle to determine if grounded
	public bool m_Grounded;             // Whether or not the player is grounded.
	private bool m_FacingRight = true;   // For determining which way the player is currently facing.
	public bool m_hasPlatformBeneath;
	private Vector2 m_Velocity = Vector2.zero;
	private Rigidbody2D m_rb2d;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	public UnityEvent<bool> OnCrouchEvent;
	// [System.Serializable]
	// public class BoolEvent : UnityEvent<bool> { }
	// public BoolEvent OnCrouchEvent;

	void Start() {
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
			if (safeColliders[i].gameObject != gameObject) {
				if (!safeColliders[i].isTrigger) { // this "if block" is needed. Otherwise, player would be able to jump while it is on triggered colliders in platforms. 
					m_Grounded = true;
				}
				if (!wasGrounded) {
					OnLandEvent.Invoke();
				}
			}
		}
		if (!m_Grounded) {
			animator.SetBool("isJumping", true);
		}
	}

	public void Move(float moveX, float moveY, bool crouch, bool jump, bool lookingUp, bool climb) {
		// Only control the player if grounded or airControl is turned on while the player is not knockbacked and not looking up
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
				// Enable the collider when not crouching
				if (m_CrouchDisableCollider != null) {
					if (!m_hasPlatformBeneath) {
						m_CrouchDisableCollider.enabled = true;
					}
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
			}

			if (moveX < 0 && m_FacingRight) {
				FlipThePlayer();
			} else if (moveX > 0 && !m_FacingRight) {
				FlipThePlayer();
			}
		} else if (m_knockbackCounter > 0) {
			animator.SetBool("isHurt", true);
			if (m_isCollidedWithLethalArea) {
				m_rb2d.linearVelocity = new Vector2(m_rb2d.linearVelocity.x, k_lethalAreaKnockBackForce);
				m_isCollidedWithLethalArea = false;
			} else if (m_isCollidedWithEnemy) {
				if (m_knockbackedFromRight) {
					Vector2 targetKnockbackForce = new Vector2(-k_enemyKnockbackForceX, k_enemyKnockbackForceY);
					m_rb2d.linearVelocity = Vector2.SmoothDamp(m_rb2d.linearVelocity, targetKnockbackForce, ref m_Velocity, m_MovementSmoothing);
				}
				if (!m_knockbackedFromRight) {
					Vector2 targetKnockbackForce = new Vector2(k_enemyKnockbackForceX, k_enemyKnockbackForceY);
					m_rb2d.linearVelocity = Vector2.SmoothDamp(m_rb2d.linearVelocity, targetKnockbackForce, ref m_Velocity, m_MovementSmoothing);
				}
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

    private void OnDrawGizmos()	{
		if (m_GroundCheck != null) {
			Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
		}
    }
}