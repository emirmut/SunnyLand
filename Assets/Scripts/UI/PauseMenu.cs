using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private SceneManager sceneManager;
    [SerializeField] private AudioManager audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() 
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Resume() {
        audioManager.PlaySFXOneShot(audioManager.buttonClicked);
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    public void Quit() {
        audioManager.PlaySFXOneShot(audioManager.buttonClicked);
        Application.Quit();
    }

    public void Levels() {
        audioManager.PlaySFXOneShot(audioManager.buttonClicked);
        sceneManager.OnSceneSwitch("Levels");
    }
}
