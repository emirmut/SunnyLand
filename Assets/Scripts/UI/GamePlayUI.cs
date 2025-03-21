using UnityEngine;

public class GamePlayUI : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private AudioManager audioManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseButtonClicked() {
        audioManager.PlaySFXOneShot(audioManager.gamePaused);
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }
}
