using UnityEngine;
using UnityEngine.Events;

public class EnemyHeadChecker : MonoBehaviour
{
    [SerializeField] private Rigidbody2D player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            player.AddForce(new Vector2(0f, 600f));
            enemy.Die();
        }
    }
}
