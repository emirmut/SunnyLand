using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private Collider2D boxCollider0;
    [SerializeField] private Collider2D boxCollider1;
    [SerializeField] private Collider2D capsuleCollider0;
    [SerializeField] private Collider2D capsuleCollider1;
    [SerializeField] protected Sprite frogIdle;
    [SerializeField] protected Sprite frogAscending;
    [SerializeField] protected Sprite frogDescending;
    [SerializeField] private Animator frogAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        colliders = new Collider2D[] { boxCollider0, boxCollider1, capsuleCollider0, capsuleCollider1 };
        animator = frogAnimator;
        sprites = new Sprite[] { frogIdle, frogAscending, frogDescending };
        Debug.Log("Frog Start method called");
    }
}
