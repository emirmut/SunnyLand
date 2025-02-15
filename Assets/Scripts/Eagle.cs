using UnityEngine;

public class Eagle : Enemy
{
    [SerializeField] private Collider2D polygonCollider0;
    [SerializeField] private Collider2D polygonCollider1;
    [SerializeField] private Collider2D boxCollider;
    [SerializeField] private Collider2D capsuleCollider;
    [SerializeField] private Animator eagleAnimator;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        colliders = new Collider2D[] { polygonCollider0, polygonCollider1, boxCollider, capsuleCollider };
        animator = eagleAnimator;
    }

    public void OnEagleDie() {
        OnDie();
    }
}
