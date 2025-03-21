using UnityEngine;

public class Opposum : Enemy
{
    [SerializeField] private Collider2D boxCollider0;
    [SerializeField] private Collider2D boxCollider1;
    [SerializeField] private Collider2D boxCollider2;
    [SerializeField] private Collider2D capsuleCollider;
    [SerializeField] private Animator opposumAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        colliders = new Collider2D[] { boxCollider0, boxCollider1, boxCollider2, capsuleCollider };
        animator = opposumAnimator;
    }
}
