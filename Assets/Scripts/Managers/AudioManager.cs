using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [Space]
    public AudioSource musicSource;
    public AudioSource SFXSource;
    public AudioSource runningSource;
    public AudioSource crouchingSource;
    public AudioSource climbingSource;
    [Space]
    
    [Header("Audio Clips")]
    [Space]
    public AudioClip level1BackgroundMusic; //
    public AudioClip level2BackgroundMusic; 
    public AudioClip buttonClicked; //
    public AudioClip buttonHovered; //
    public AudioClip collect; //
    public AudioClip dialogueButtonClicked; //
    public AudioClip enemyDeath; //
    public AudioClip enemyKnockback; //
    public AudioClip gameOver; //
    public AudioClip gamePaused; //
    public AudioClip itemInteractionDialogue; //
    public AudioClip jump; //
    public AudioClip levelButtonClicked; //
    public AudioClip levelButtonHover; //
    public AudioClip liftCrank; //
    public AudioClip lowerCrank; //
    public AudioClip optionsElementClicked; //
    public AudioClip spikeInteraction; //
    public AudioClip toggleButtonOn; //

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "StartMenu" && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Levels") {
            MenuBackgroundMusicManager.instance.GetComponent<AudioSource>().Stop();
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level1") {
                musicSource.clip = level1BackgroundMusic;
            } else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Level2") {
                musicSource.clip = level2BackgroundMusic;
            }
            musicSource.Play();
        } else {
            musicSource.Stop();
            if (!MenuBackgroundMusicManager.instance.GetComponent<AudioSource>().isPlaying) {
                MenuBackgroundMusicManager.instance.GetComponent<AudioSource>().Play();
            }
        }
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void PlaySFXOneShot(AudioClip audioClip) {
        SFXSource.PlayOneShot(audioClip);
    }
}
