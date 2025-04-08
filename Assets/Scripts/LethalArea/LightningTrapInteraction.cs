using System.Collections;
using UnityEngine;

public class LightningTrapInteraction : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private AudioManager audioManager;
    private Animator animator;
    private Collider2D triggeredCollider;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        animator = GetComponent<Animator>();
        triggeredCollider = GetComponent<Collider2D>();
        triggeredCollider.enabled = false;
        StartCoroutine(LightningTrapAnimation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            controller.m_isCollidedWithLightningTrap = true;
            controller.m_knockbackCounter = controller.k_knockbackLength;
            controller.m_lives--;
            controller.HealthUpdate();
            audioManager.PlaySFXOneShot(audioManager.lightningTrapInteraction);
        }
    }

    private IEnumerator LightningTrapAnimation() {
        while (true) {
            yield return new WaitForSeconds(2f);
            animator.SetTrigger("startLightningAnimation");
            triggeredCollider.enabled = true;
            yield return new WaitForSeconds(1f);
            animator.SetTrigger("endLightningAnimation");
            triggeredCollider.enabled = false;
        }
    }
}
