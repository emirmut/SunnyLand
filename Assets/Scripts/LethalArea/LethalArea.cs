using UnityEngine;

public class LethalArea : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private AudioManager audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        GameObject player = collision.gameObject;
        if (player != null && player.CompareTag("Player")) {
            controller.m_isCollidedWithLethalArea = true;
            controller.m_knockbackCounter = controller.k_knockbackLength;
            controller.m_lives--;
            controller.HealthUpdate();
            audioManager.PlaySFXOneShot(audioManager.spikeInteraction);
            if (transform.position.y <= player.transform.position.y) {
                controller.m_knockbackedFromUp = false;
            } else {
                controller.m_knockbackedFromUp = true;
            }
        }
    }
}
