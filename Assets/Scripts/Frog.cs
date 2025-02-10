using UnityEngine;

public class Frog : Enemy
{
    [SerializeField] private Collider2D boxCollider0;
    [SerializeField] private Collider2D boxCollider1;
    [SerializeField] private Collider2D capsuleCollider0;
    [SerializeField] private Collider2D capsuleCollider1;
    [SerializeField] private Animator frogAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        colliders = new Collider2D[] { boxCollider0, boxCollider1, capsuleCollider0, capsuleCollider1 };
        animator = frogAnimator;
    }

    public void OnFrogDie() {
        OnDie();
    }
}
