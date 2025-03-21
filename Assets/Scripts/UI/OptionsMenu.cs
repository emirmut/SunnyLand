using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer menuMusicAudioMixer;
    public AudioMixer inGameMusicVolumeAudioMixer;
    public AudioMixer sfxVolumeAudioMixer;
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    [SerializeField] private CharacterController controller;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private AudioManager audioManager; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMainMenuMusicVolume(float volume) {
        menuMusicAudioMixer.SetFloat("mainMenuMusicVolume", volume);
    }
    
    public void SetMusicVolume(float volume) {
        inGameMusicVolumeAudioMixer.SetFloat("inGameMusicVolume", volume);
    }

    public void SetSFXVolume(float volume) {
        sfxVolumeAudioMixer.SetFloat("sfxVolume", volume);
    }

    public void SetQuality(int qualityIndex) {
       QualitySettings.SetQualityLevel(qualityIndex); 
    }

    public void SetFullscreen(bool isFullScreen) {
        if (isFullScreen) {
            audioManager.PlaySFXOneShot(audioManager.optionsElementClicked);
        } else {
            audioManager.PlaySFXOneShot(audioManager.toggleButtonOn);
        }
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void BackButtonClicked() {
        if (controller.m_lives > 0) {
            pauseMenu.SetActive(true);
        } else {
            gameOverScreen.SetActive(true);
        }
    }
}

