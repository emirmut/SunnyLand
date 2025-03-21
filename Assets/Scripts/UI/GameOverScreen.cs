using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SceneManager sceneManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameOver() {
        Time.timeScale = 0;
        gameOverScreen.SetActive(true);
        audioManager.PlaySFXOneShot(audioManager.gameOver);
    }

    public void Restart() {
        Time.timeScale = 1;
        sceneManager.OnSceneSwitch(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        audioManager.PlaySFXOneShot(audioManager.buttonClicked);
    }

    public void BackToStartMenu() {
        audioManager.PlaySFXOneShot(audioManager.buttonClicked);
        sceneManager.OnSceneSwitch("StartMenu");
    }
}
