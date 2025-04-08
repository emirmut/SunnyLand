using UnityEngine;

[System.Serializable] // this allows us to save PlayerData class in a file
public class PlayerData
{
    public int level;
    public int totalCollectedStars;
    public int[] collectedStarsOnCurrentLevel;

    public PlayerData(SaveAndLoadManager saveAndLoadManager) {
        level = saveAndLoadManager.level;
        totalCollectedStars = saveAndLoadManager.totalCollectedStars;
        collectedStarsOnCurrentLevel = saveAndLoadManager.collectedStarsOnCurrentLevel;
    }
}
