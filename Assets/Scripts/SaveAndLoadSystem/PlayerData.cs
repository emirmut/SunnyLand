using UnityEngine;

[System.Serializable] // this allows us to save PlayerData class in a file
public class PlayerData
{
    public int level;

    public PlayerData(SaveAndLoadManager saveAndLoadManager) {
        level = saveAndLoadManager.level;
    }
}
