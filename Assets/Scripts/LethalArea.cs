using Unity.VisualScripting;
using UnityEngine;

public class LethalArea : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

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
        }
    }
}
