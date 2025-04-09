using TMPro;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] protected Animator animator;
    protected const float destroyDuration = 0.5f;
    [SerializeField] protected AudioManager audioManager;
    [SerializeField] protected TextMeshProUGUI cherryCounterText;
    [SerializeField] protected TextMeshProUGUI gemCounterText;
    private bool isCollected = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (isCollected) {
            return;
        }
        if (collision.CompareTag("Player")) {
            isCollected = true;
            animator.SetBool("isCollected", true);
            audioManager.PlaySFXOneShot(audioManager.collect);

            if (int.TryParse(cherryCounterText.text, out int cherryCount) && gameObject.CompareTag("Cherry")) {
                cherryCounterText.text = (cherryCount + 1).ToString();
            }
            if (int.TryParse(gemCounterText.text, out int gemCount) && gameObject.CompareTag("Gem")) {
                gemCounterText.text = (gemCount + 1).ToString();
            }

            Destroy(gameObject, destroyDuration);
        }
}   
}
