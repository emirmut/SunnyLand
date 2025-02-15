using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private BoxCollider2D disabledCollider0;
    [SerializeField] private CircleCollider2D disabledCollider1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Platform")) {
            controller.m_hasPlatformBeneath = true;
            controller.m_Grounded = false; // set to false because otherwise, player would be able to jump while it is on triggered colliders in platforms. 
            disabledCollider0.enabled = false;
            disabledCollider1.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Platform")) {
            controller.m_hasPlatformBeneath = false;
            disabledCollider0.enabled = true;
            disabledCollider1.enabled = true;
        }
    }
}
