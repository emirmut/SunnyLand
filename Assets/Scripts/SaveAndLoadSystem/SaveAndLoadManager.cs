using System.IO;
using UnityEngine;

public class SaveAndLoadManager : MonoBehaviour
{
    [HideInInspector] public int level = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        string path = Application.persistentDataPath + "/player.save";
        if (!File.Exists(path)) {
            SavePlayer();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SavePlayer() {
        Debug.Log("Saving current level: " + level);
        SaveSystem.SavePlayer(this);
    }

    public void LoadPlayer() {
        Debug.Log("LoadPlayer function called");
        PlayerData data = SaveSystem.LoadPlayer();
        if (data != null) {
            level = data.level;
            Debug.Log("Player data loaded: Level " + level);
        } else {
            Debug.LogError("Failed to load player data");
        }
    }

    public void DeletePlayer() {
        Debug.Log("DeletePlayer function called");
        SaveSystem.DeletePlayer();
    }
}
