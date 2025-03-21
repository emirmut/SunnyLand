using Unity.VisualScripting;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    protected const float destroyDuration = 0.5f;
    [SerializeField] protected AudioManager audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            animator.SetBool("isCollected", true);
            audioManager.PlaySFXOneShot(audioManager.collect);
            Destroy(gameObject, destroyDuration);
        }
    }
}
