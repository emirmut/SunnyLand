using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    protected Rigidbody2D rb2d;
    protected Collider2D[] colliders;
    protected SpriteRenderer sr;
    protected Sprite[] sprites;
    protected Animator animator;
    protected const float deathAnimationTime = 0.5f;
    [SerializeField] protected GameObject[] patrolPoints;
    protected Transform destinationPoint;
    [SerializeField] protected float velocity;
    protected bool wasFlipped;
    [SerializeField] private AudioManager audioManager;

    [Header("Events")]
    [Space]
    public UnityEvent OnDeathEvent;

    protected virtual void Start() { // What virtual keyword means is that this method can be overridden by any class that inherits it
        if (OnDeathEvent == null) {
            OnDeathEvent = new UnityEvent();
        }
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        if (!gameObject.CompareTag("Ghost")) {
            if (!gameObject.CompareTag("Frog")) {
                destinationPoint = patrolPoints[1].transform;
            } else {
                destinationPoint = patrolPoints[2].transform;
            }
        }
        StartCoroutine(Patrol());
    }

    protected virtual void OnTriggerEnter2D(Collider2D other) {
        var characterController = other.GetComponent<CharacterController>();
        if (characterController) {
            characterController.m_isCollidedWithEnemy = true;
            characterController.m_lives--;
            characterController.HealthUpdate();
            characterController.m_knockbackCounter = characterController.k_knockbackLength;
            audioManager.PlaySFXOneShot(audioManager.enemyKnockback);
            if (other.transform.position.x < transform.position.x) {
                characterController.m_knockbackedFromRight = true;
            } else {
                characterController.m_knockbackedFromRight = false;
            }
        }
    }

    protected virtual IEnumerator Patrol() {
        Debug.Log("Patrol method called");
        while (true) {
            if (destinationPoint == patrolPoints[0].transform) {
                if (gameObject.CompareTag("Opposum")) {
                    rb2d.linearVelocity = new Vector2(velocity, rb2d.linearVelocity.y);
                }
                if (gameObject.CompareTag("Eagle") || gameObject.CompareTag("Owl")) {
                    rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, -velocity);
                }
                if (gameObject.CompareTag("Frog")) {
                    animator.SetBool("isIdle", false);
                    sr.sprite = sprites[2];
                    rb2d.linearVelocity = new Vector2(velocity, -velocity);
                    Debug.Log("Frog has + " + rb2d.linearVelocity + " velocity.");
                }

                if (Vector2.Distance(transform.position, destinationPoint.position) < 0.5f) {
                    if (gameObject.CompareTag("Opposum")) {
                        StartCoroutine(Flip());
                    }
                    if (gameObject.CompareTag("Opposum") || gameObject.CompareTag("Eagle") || gameObject.CompareTag("Owl")) {
                        destinationPoint = patrolPoints[1].transform;
                    }
                    if (gameObject.CompareTag("Frog")) {
                        sr.sprite = sprites[0];
                        animator.SetBool("isIdle", true);
                        if (!wasFlipped) {
                            wasFlipped = true;
                            StartCoroutine(Flip());
                        }
                        yield return new WaitForSeconds(2f);
                        wasFlipped = false;
                        destinationPoint = patrolPoints[2].transform;
                    }
                }
            } else if (destinationPoint == patrolPoints[1].transform) {
                if (gameObject.CompareTag("Opposum")) {
                    rb2d.linearVelocity = new Vector2(-velocity, rb2d.linearVelocity.y);
                }
                if (gameObject.CompareTag("Eagle") || gameObject.CompareTag("Owl")) {
                    rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x, velocity);
                }
                if (gameObject.CompareTag("Frog")) {
                    animator.SetBool("isIdle", false);
                    sr.sprite = sprites[2];
                    rb2d.linearVelocity = new Vector2(-velocity, -velocity);
                    Debug.Log("Frog has + " + rb2d.linearVelocity + " velocity.");
                }
    
                if (Vector2.Distance(transform.position, destinationPoint.position) < 0.5f) {
                    if (gameObject.CompareTag("Opposum")) {
                        StartCoroutine(Flip());
                    }
                    if (gameObject.CompareTag("Opposum") || gameObject.CompareTag("Eagle") || gameObject.CompareTag("Owl")) {
                        destinationPoint = patrolPoints[0].transform;
                    }
                    if (gameObject.CompareTag("Frog")) {
                        sr.sprite = sprites[0];
                        animator.SetBool("isIdle", true);
                        if (!wasFlipped) {
                            wasFlipped = true;
                            StartCoroutine(Flip());
                        }
                        yield return new WaitForSeconds(2f);
                        wasFlipped = false;
                        destinationPoint = patrolPoints[2].transform;
                    }
                }
            } else if (destinationPoint == patrolPoints[2].transform) {
                if (gameObject.CompareTag("Frog")) {
                    animator.SetBool("isIdle", false);
                    sr.sprite = sprites[1];
                    if (transform.localScale.x > 0) {
                        rb2d.linearVelocity = new Vector2(-velocity, velocity);
                        Debug.Log("Frog has + " + rb2d.linearVelocity + " velocity.");
                    } else {
                        rb2d.linearVelocity = new Vector2(velocity, velocity);
                        Debug.Log("Frog has + " + rb2d.linearVelocity + " velocity.");
                    }
                    if (Vector2.Distance(transform.position, destinationPoint.position) < 0.5f) {
                        sr.sprite = sprites[2];
                        if (transform.localScale.x > 0) {
                            destinationPoint = patrolPoints[1].transform;
                        } else {
                            destinationPoint = patrolPoints[0].transform;
                        }
                    }
                }
            }
            yield return null;
        }
    }

    protected virtual IEnumerator Flip() {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        yield return null;
    }

    public void OnDie() {
        if (colliders != null && colliders.Length > 0) {
            rb2d.bodyType = RigidbodyType2D.Static;
            foreach (var collider in colliders) {
                collider.enabled = false;
            }
            animator.SetTrigger("isDead");
            audioManager.PlaySFXOneShot(audioManager.enemyDeath);
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
