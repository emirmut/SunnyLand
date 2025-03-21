using System.Collections;
using UnityEngine;

public class Ghost : Enemy
{
    [SerializeField] private Collider2D boxCollider0;
    [SerializeField] private Collider2D boxCollider1;
    [SerializeField] private Collider2D capsuleCollider0;
    [SerializeField] private Collider2D capsuleCollider1;
    [SerializeField] private Animator ghostAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
        colliders = new Collider2D[] { boxCollider0, boxCollider1, capsuleCollider0, capsuleCollider1 };
        animator = ghostAnimator;
        StartCoroutine(Flip());
    }

    protected override IEnumerator Flip() {
        while (true) {
            yield return new WaitForSeconds(2f);
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
}
