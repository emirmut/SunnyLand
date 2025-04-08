using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelButtonManager : MonoBehaviour
{
    [SerializeField] private SaveAndLoadManager saveAndLoadManager;
    [HideInInspector] private Transform[] levels;
    [SerializeField] private Sprite yellowStar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        saveAndLoadManager.LoadPlayer();
        int completedLevels = saveAndLoadManager.level;
        int[] collectedStarsOnCurrentLevel = saveAndLoadManager.collectedStarsOnCurrentLevel;
        
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
            Image[] stars = levels[i].GetComponentsInChildren<Image>().Skip(2).Take(3).ToArray(); // skips the first 2 image components and takes the next 3 image components in the children of levels[i]
            int buttonLevel = int.Parse(buttonText.text);
            if (buttonLevel > completedLevels) { // if the player hasn't reached that buttonLevel yet
                button.interactable = false;
                lockImage.enabled = true;
                buttonText.enabled = false;
                backgroundColor.a = 150/255f;
                background.color = backgroundColor;
                animator.enabled = false;
                eventTrigger.enabled = false;
                foreach (Image star in stars) {
                    star.enabled = false;
                }
            } else {
                button.interactable = true;
                lockImage.enabled = false;
                buttonText.enabled = true;
                backgroundColor.a = 1f;
                background.color = backgroundColor;
                animator.enabled = true;
                eventTrigger.enabled = true;
                foreach (Image star in stars) {
                    star.enabled = true;
                }
                if (i < collectedStarsOnCurrentLevel.Length) {
                    if (collectedStarsOnCurrentLevel[i] == 3) {
                        foreach (Image star in stars) {
                            star.sprite = yellowStar;
                        }
                    } else if (collectedStarsOnCurrentLevel[i] == 2) {
                        foreach (Image star in stars.Take(2)) {
                            star.sprite = yellowStar;
                        }
                    } else {
                        foreach (Image star in stars.Take(1)) {
                            star.sprite = yellowStar;
                        }
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update() 
    {
        
    }
}
