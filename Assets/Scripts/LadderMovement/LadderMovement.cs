using UnityEngine;

public class LadderMovement : MonoBehaviour
{
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (gameObject.CompareTag("Ladder")) {
            animator.SetBool("isClimbing", true);
		}
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (gameObject.CompareTag("Ladder")) {
			playerMovement.climb = false;
            animator.SetBool("isClimbing", false);
		}
    }
}
