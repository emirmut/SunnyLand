using UnityEngine;
using System.IO; // is used whenever we want to work with files in our operating system
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem // this class is static because we don't want this class to be instantiated
{                              // we don't want to accidentally create multiple versions of the save system
    public static void SavePlayer(SaveAndLoadManager saveAndLoadManager) { // it is static so that we can call it anywhere without an instance
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.save"; // C:/Users/Hp/AppData/LocalLow/DefaultCompany/SunnyLand/player.save
        FileStream fileStream = new FileStream(path, FileMode.Create);
        PlayerData playerData = new PlayerData(saveAndLoadManager);
        Debug.Log("Saving level: " + playerData.level);
        binaryFormatter.Serialize(fileStream, playerData);
        fileStream.Close();
        Debug.Log("Game saved to " + path);
    }

    public static PlayerData LoadPlayer() {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path)) {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            PlayerData playerData = (PlayerData) binaryFormatter.Deserialize(fileStream);
            fileStream.Close();
            return playerData;
        } else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeletePlayer() {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path)) {
            File.Delete(path);
            Debug.Log("Save file deleted from " + path);
        } else {
            Debug.LogError("Save file not found in " + path);
        }
    }
}
