using UnityEngine;

public class Door : Interactable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update() {
        base.Update();
    }

    public void SwitchSpriteV1() {
        gameObject.GetComponent<SpriteRenderer>().sprite = interactableV1;
    }

    public void SwitchSpriteV2() {
        gameObject.GetComponent<SpriteRenderer>().sprite = interactableV2;
    }
}
