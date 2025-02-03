using UnityEngine;
using UnityEngine.Events;

public class CharacterController : MonoBehaviour {
	[SerializeField] private float m_JumpForce = 230f;							// Amount of force added when the player jumps.
	[Range(0, 1)] [SerializeField] private float m_CrouchSpeed = 0.4f;			// Amount of maxSpeed applied to crouching movement. 1 = 100%
	[Range(0, 0.3f)] [SerializeField] private float m_MovementSmoothing = 0.05f;// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = true;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	const float k_GroundedRadius = 0.2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;             // Whether or not the player is grounded.
	const float k_CeilingRadius = 0.2f;  // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_rb2d;
	private bool m_FacingRight = true;   // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;
	public UnityEvent<bool> OnCrouchEvent;
	// [System.Serializable]
	// public class BoolEvent : UnityEvent<bool> { }
	// public BoolEvent OnCrouchEvent;
	private bool m_wasCrouching = false;

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
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++) {
			if (colliders[i].gameObject != gameObject) {
				m_Grounded = true;
				if (!wasGrounded) {
					OnLandEvent.Invoke();
				}
			}
		}
	}

	public void Move(float move, bool crouch, bool jump) {
		if (!crouch) {
			// If the character has a ceiling preventing them from standing up, keep them crouching
			if (Physics2D.OverlapCircle(m_CeilingCheck.position, k_CeilingRadius, m_WhatIsGround)) {
				crouch = true;
			}
		}

		// Only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl) {
			if (crouch) {
				if (!m_wasCrouching) {
					m_wasCrouching = true;
					OnCrouchEvent.Invoke(true);
				}
				move *= m_CrouchSpeed;
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
					m_CrouchDisableCollider.enabled = true;
				}
			}

			Vector3 targetVelocity = new Vector2(move, m_rb2d.linearVelocity.y);
			m_rb2d.linearVelocity = Vector3.SmoothDamp(m_rb2d.linearVelocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
			if (move < 0 &&  m_FacingRight) {
				FlipThePlayer();
			} else if (move > 0 && !m_FacingRight) {
				FlipThePlayer();
			}

			if (m_Grounded && jump) {
				m_Grounded = false;
				m_rb2d.AddForce(new Vector2(0f, m_JumpForce));
			}
		}
	}

	private void FlipThePlayer() {
		m_FacingRight = !m_FacingRight;
		// Multiply the player's x local scale by -1 to mirror the player's sprite along the vertical axis.
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
	}
}