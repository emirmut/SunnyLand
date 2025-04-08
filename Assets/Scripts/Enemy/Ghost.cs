using System.Collections;
using UnityEngine;

public class Ghost : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
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
