using UnityEngine;

public class Owl : Enemy
{
    [SerializeField] private Collider2D capsuleCollider0;
    [SerializeField] private Collider2D capsuleCollider1;
    [SerializeField] private Collider2D capsuleCollider2;
    [SerializeField] private Collider2D capsuleCollider3;
    [SerializeField] private Animator owlAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        colliders = new Collider2D[] { capsuleCollider0, capsuleCollider1, capsuleCollider2, capsuleCollider3 };
        animator = owlAnimator;
    }

    public void OnOwlDie() {
        OnDie();
    }
}
