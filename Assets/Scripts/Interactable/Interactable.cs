using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Collider2D))] // every interactable object needs a Collider2D
public class Interactable : MonoBehaviour
{
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SaveAndLoadManager saveAndLoadManager;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Sprite interactableV1;
    [SerializeField] protected Sprite interactableV2;
    [SerializeField] protected Door door;
    [SerializeField] protected Animator animator;
    protected bool playerDetected;
    [SerializeField] protected GameObject interactionPanel;
    [SerializeField] protected Text interactionText;
    [SerializeField] protected string[] lines; // replikler
    protected int lineIndex = 0; // demonstrates which line (replik) we are currently in
    [SerializeField] protected float appearingDelay;
    [SerializeField] private GameObject continueButton;
    private Coroutine typingCoroutine;

    [Space]
    [Header("Events")]
    [SerializeField] protected UnityEvent OnSceneSwitchEvent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (OnSceneSwitchEvent == null) {
            OnSceneSwitchEvent = new UnityEvent();
        }
    }

    protected virtual void Update() {
        Interact();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerDetected = true;
            interactionPanel.SetActive(true);
            StartTypingCurrentLine();
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            playerDetected = false;
            ResetText();
        }
    }

    protected virtual void Interact() {
        if (playerDetected && Input.GetButtonDown("Interact")) {
            if (gameObject.CompareTag("Door") && spriteRenderer.sprite == interactableV2) {
                OnSceneSwitchEvent.Invoke();
                saveAndLoadManager.level++;
                saveAndLoadManager.SavePlayer();
                Debug.Log("Next Level");
            }
            if (gameObject.CompareTag("Crank")) {
                if (spriteRenderer.sprite == interactableV1) {
                    spriteRenderer.sprite = interactableV2;
                    audioManager.PlaySFXOneShot(audioManager.lowerCrank);
                    if (door != null) {
                        door.SwitchSpriteV2();
                    }
                    if (animator != null) {
                        animator.SetBool("isObstacleVanished", true);
                    }
                } else if (spriteRenderer.sprite == interactableV2) {
                    spriteRenderer.sprite = interactableV1;
                    audioManager.PlaySFXOneShot(audioManager.liftCrank);
                    if (door != null) {
                        door.SwitchSpriteV1();
                    }
                    if (animator != null) {
                        animator.SetBool("isObstacleVanished", false);
                    }
                }
            }
        }
    }

    protected virtual void ResetText() { // returns back to the values before entering the triggered collider
        interactionText.text = "";
        lineIndex = 0;
        interactionPanel.SetActive(false);
        if (audioManager.SFXSource.isPlaying) {
            audioManager.SFXSource.Stop();
        }
    }

    protected virtual IEnumerator Typing() {
        foreach (char letter in lines[lineIndex].ToCharArray()) {
            interactionText.text += letter;
            yield return new WaitForSeconds(appearingDelay);
        }
        continueButton.SetActive(true); // make continueButton active after the entire line (replik) has been written in the interactionText
    }

    protected virtual void StartTypingCurrentLine() {
        if (typingCoroutine != null) { // if a line (replik) is being written (if there is a reference to the Typing() coroutine)
            StopCoroutine(typingCoroutine); // stop writing that line
        }
        interactionText.text = "";
        continueButton.SetActive(false);
        typingCoroutine = StartCoroutine(Typing());
        audioManager.PlaySFXOneShot(audioManager.itemInteractionDialogue);
    }

    protected virtual void NextLine() {
        if (audioManager.SFXSource.isPlaying) {
            audioManager.SFXSource.Stop();
        }
        continueButton.SetActive(false); // make continueButton inactive right after the continueButton is clicked
        if (lineIndex < lines.Length - 1) { // if the current line is not the last line
            lineIndex++;
            StartTypingCurrentLine();
        } else { // if the current line is the last line
            ResetText(); // return back to the values before entering the triggered collider
        }
    }
}
