using System.Collections;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb2d;
    protected Collider2D[] colliders;
    protected Animator animator;
    protected const float deathAnimationTime = 0.5f;
    [SerializeField] protected GameObject[] patrolPoints;
    protected Transform destinationPoint;
    [SerializeField] protected float velocity;

    [Header("Events")]
    [Space]
    public UnityEvent OnDeathEvent;

    protected virtual void Start() { // What virtual keyword means is that this method can be overridden by any class that inherits it
        if (OnDeathEvent == null) {
            OnDeathEvent = new UnityEvent();
        }
        rb2d = GetComponent<Rigidbody2D>();
        if (!gameObject.CompareTag("Frog")) {
            destinationPoint = patrolPoints[1].transform;
        } else {
            destinationPoint = patrolPoints[2].transform;
        }
    }

    public virtual void Die() {
        OnDeathEvent.Invoke();
    }

    protected virtual void FixedUpdate() {
        Patrol();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        var characterController = other.GetComponent<CharacterController>();
        if (characterController) {
            characterController.m_lives--;
            characterController.m_knockbackCounter = characterController.k_knockbackLength;
            if (other.transform.position.x < transform.position.x) {
                characterController.m_knockbackedFromRight = true;
            } else {
                characterController.m_knockbackedFromRight = false;
            }
        }
    }

    protected virtual void Patrol() {
        if (destinationPoint == patrolPoints[0].transform) {
            if (gameObject.CompareTag("Opposum")) {
                rb2d.linearVelocity = new Vector2(velocity, rb2d.linearVelocity.y);
            }
            if (gameObject.CompareTag("Eagle")) {
                rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, -velocity);
            }
            if (gameObject.CompareTag("Frog")) {
                animator.SetBool("isJumpAscending", false);
                animator.SetBool("isJumpDescending", true);
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].transform.position, velocity * Time.fixedDeltaTime);
            }

            if (Vector2.Distance(transform.position, destinationPoint.position) < 0.5f) {
                if (gameObject.CompareTag("Opposum")) {
                    Flip();
                }
                if (gameObject.CompareTag("Opposum") || gameObject.CompareTag("Eagle")) {
                    destinationPoint = patrolPoints[1].transform;
                }
                if (gameObject.CompareTag("Frog")) {
                    animator.SetBool("isJumpAscending", false);
                    animator.SetBool("isJumpDescending", false);
                    Flip();
                    destinationPoint = patrolPoints[2].transform;
                }
            }
        } else if (destinationPoint == patrolPoints[1].transform) {
            if (gameObject.CompareTag("Opposum")) {
                rb2d.linearVelocity = new Vector2(-velocity, rb2d.linearVelocity.y);
            }
            if (gameObject.CompareTag("Eagle")) {
                rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, velocity);
            }
            if (gameObject.CompareTag("Frog")) {
                animator.SetBool("isJumpAscending", false);
                animator.SetBool("isJumpDescending", true);
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].transform.position, velocity * Time.fixedDeltaTime);
            }

            if (Vector2.Distance(transform.position, destinationPoint.position) < 0.5f) {
                if (gameObject.CompareTag("Opposum")) {
                    Flip();
                }
                if (gameObject.CompareTag("Opposum") || gameObject.CompareTag("Eagle")) {
                    destinationPoint = patrolPoints[0].transform;
                }
                if (gameObject.CompareTag("Frog")) {
                    animator.SetBool("isJumpAscending", false);
                    animator.SetBool("isJumpDescending", false);
                    Flip();
                    destinationPoint = patrolPoints[2].transform;
                }
            }
        } else if (destinationPoint == patrolPoints[2].transform) {
            if (gameObject.CompareTag("Frog")) {
                animator.SetBool("isJumpAscending", true);
                animator.SetBool("isJumpDescending", false);
                transform.position = Vector2.MoveTowards(transform.position, patrolPoints[2].transform.position, velocity * Time.fixedDeltaTime);
                if (Vector2.Distance(transform.position, destinationPoint.position) < 0.5f) {
                    if (transform.localScale.x > 0) {
                        destinationPoint = patrolPoints[1].transform;
                    } else {
                        destinationPoint = patrolPoints[0].transform;
                    }
                }
            }
        }
    }

    protected virtual void Flip() {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    protected void OnDie() {
        if (colliders != null && colliders.Length > 0) {
            rb2d.bodyType = RigidbodyType2D.Static;
            foreach (var collider in colliders) {
                collider.enabled = false;
            }
            animator.SetTrigger("isDead");
            Destroy(gameObject, deathAnimationTime);
        }
    }

    protected virtual void OnDrawGizmos() {
        if (patrolPoints[0] != null && patrolPoints[1] != null) {
            Gizmos.DrawWireSphere(patrolPoints[0].transform.position, 0.5f);
            Gizmos.DrawWireSphere(patrolPoints[1].transform.position, 0.5f);
            Gizmos.DrawLine(patrolPoints[0].transform.position, patrolPoints[1].transform.position);
        }
        if (patrolPoints[2] != null) {
            Gizmos.DrawWireSphere(patrolPoints[2].transform.position, 0.5f);
            Gizmos.DrawLine(patrolPoints[0].transform.position, patrolPoints[2].transform.position);
            Gizmos.DrawLine(patrolPoints[1].transform.position, patrolPoints[2].transform.position);
        }
    }
}
