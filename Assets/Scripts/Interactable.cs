using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    protected bool playerDetected;
    [SerializeField] protected Transform interactableTransform;
    [SerializeField] protected float width;
    [SerializeField] protected float height;
    [SerializeField] protected LayerMask whatIsPlayer;
    [SerializeField] protected Sprite interactableV1;
    [SerializeField] protected Sprite interactableV2;
    [SerializeField] protected Door door;
    [SerializeField] protected Animator animator;

    [Space]
    [Header("Events")]
    [SerializeField] protected UnityEvent OnSceneSwitchEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start() {
        if (OnSceneSwitchEvent == null) {
            OnSceneSwitchEvent = new UnityEvent();
        }
    }

    // Update is called once per frame
    protected virtual void Update() {
        if (playerDetected) {
            if (Input.GetButtonDown("Interact")) {
                if (gameObject.CompareTag("Door") && gameObject.GetComponent<SpriteRenderer>().sprite == interactableV2) {
                    // OnSceneSwitchEvent.Invoke();
                    Debug.Log("Level 2");
                }
                if (gameObject.CompareTag("Crank")) {
                    if (gameObject.GetComponent<SpriteRenderer>().sprite == interactableV1) {
                        gameObject.GetComponent<SpriteRenderer>().sprite = interactableV2;
                        if (door != null) {
                            door.SwitchSpriteV2();
                        }
                        if (animator != null) {
                            animator.SetBool("isObstacleVanished", true);
                        }
                    } else if (gameObject.GetComponent<SpriteRenderer>().sprite == interactableV2) {
                        gameObject.GetComponent<SpriteRenderer>().sprite = interactableV1;
                        if (door != null) {
                            door.SwitchSpriteV1();
                        }
                        if (animator != null) {
                            animator.SetBool("isObstacleVanished", false);
                        }
                    }
                }
            }
        }
    }

    protected virtual void FixedUpdate() {
        Collider2D[] playerColliders = Physics2D.OverlapBoxAll(interactableTransform.position, new Vector2(width, height), 0, whatIsPlayer);
        playerDetected = false;
        for (int i = 0; i < playerColliders.Length; i++) {
            if (playerColliders[i].gameObject != gameObject) {
                playerDetected = true;
            }
        }
    }

    protected virtual void OnDrawGizmos() {
        if (interactableTransform != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(interactableTransform.position, new Vector2(width, height));
        }
    }
}
