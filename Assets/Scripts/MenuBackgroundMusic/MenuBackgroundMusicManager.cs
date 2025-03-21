using UnityEngine;

public class MenuBackgroundMusicManager : MonoBehaviour
{
    public static MenuBackgroundMusicManager instance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
