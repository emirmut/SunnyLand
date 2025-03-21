using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButtonManager : MonoBehaviour
{
    private Transform[] levels;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        PlayerData data = SaveSystem.LoadPlayer();
        int completedLevels = data.level;
        
        levels = new Transform[transform.childCount];
        for (int i = 0; i < levels.Length; i++) {
            levels[i] = transform.GetChild(i);
            Animator animator = levels[i].GetComponent<Animator>();
            EventTrigger eventTrigger = levels[i].GetComponent<EventTrigger>();
            Button button = levels[i].GetComponent<Button>();
            Image background = levels[i].GetComponentsInChildren<Image>()[0];
            Color backgroundColor = background.color;
            Image lockImage = levels[i].GetComponentsInChildren<Image>()[1];
            TextMeshProUGUI buttonText = levels[i].GetComponentInChildren<TextMeshProUGUI>();
            int buttonLevel = int.Parse(buttonText.text);
            if (buttonLevel > completedLevels) {
                button.interactable = false;
                lockImage.enabled = true;
                buttonText.enabled = false;
                backgroundColor.a = 150/255f;
                background.color = backgroundColor;
                animator.enabled = false;
                eventTrigger.enabled = false;
            } else {
                button.interactable = true;
                lockImage.enabled = false;
                buttonText.enabled = true;
                backgroundColor.a = 1f;
                background.color = backgroundColor;
                animator.enabled = true;
                eventTrigger.enabled = true;
            }
        }
    }

    // Update is called once per frame
    void Update() 
    {
        
    }
}
