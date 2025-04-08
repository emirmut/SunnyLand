using UnityEngine;

public class EnemyHeadChecker : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;
    [SerializeField] private Animator animator;
    [SerializeField] private CharacterController controller;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.GetComponent<Enemy>()) {
            Enemy enemy = other.GetComponent<Enemy>(); 
            player.AddForce(new Vector2(0f, 600f));
            enemy.OnDie();
            animator.SetBool("isJumpingUp", true);
            animator.SetBool("isJumpingDown", false);
            StartCoroutine(controller.ChangeJumpAnimation());
        }
    }
}
