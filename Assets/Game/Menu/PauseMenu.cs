using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    private bool isPaused = false;

    [SerializeField] AudioMixer masterVolume;
    [SerializeField] private bool canPause;
    private void Start()
    {
        if (PlayerPrefs.GetInt("IsMuted") == 0)
        {
            masterVolume.SetFloat("MasterVolume", 0);   
        }
        else
        {
            masterVolume.SetFloat("MasterVolume", -80);
        }
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && canPause)
        {
            if(isPaused) Resume();
            else Pause();
        }
    }
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    private void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Mute()
    {
        if (PlayerPrefs.GetInt("IsMuted") == 0) 
        {
            masterVolume.SetFloat("MasterVolume", -80);
            PlayerPrefs.SetInt("IsMuted", 1);
        }
        else
        {
            masterVolume.SetFloat("MasterVolume", 0);
            PlayerPrefs.SetInt("IsMuted", 0);
        }
    }

    public void Quit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
