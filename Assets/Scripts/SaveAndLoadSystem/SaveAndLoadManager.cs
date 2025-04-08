using System.IO;
using UnityEngine;

public class SaveAndLoadManager : MonoBehaviour
{
    [HideInInspector] public int level = 1;
    [HideInInspector] public int totalCollectedStars = 0;
    [HideInInspector] public int[] collectedStarsOnCurrentLevel = new int[22]; // because there are levelButtonManager.levels.Length = 22 levels

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        string path = Application.persistentDataPath + "/player.save";
        if (!File.Exists(path)) {
            for (int i = 0; i < collectedStarsOnCurrentLevel.Length; i++) {
                collectedStarsOnCurrentLevel[i] = 0;
            }
            SavePlayer();
        } else {
            LoadPlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePlayer() {
        // Debug.Log("Saving current level: " + level);
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer() {
        // Debug.Log("LoadPlayer function called");
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null) {
            level = data.level;
            totalCollectedStars = data.totalCollectedStars;
            for (int level = 0; level < collectedStarsOnCurrentLevel.Length; level++) {
                collectedStarsOnCurrentLevel[level] = data.collectedStarsOnCurrentLevel[level];
            }
            // Debug.Log("Player data loaded: Level " + level);
        } else {
            // Debug.LogError("Failed to load player data");
        }
    }

    public void DeletePlayer() {
        // Debug.Log("DeletePlayer function called");
        SaveSystem.DeletePlayer();
    }
}
