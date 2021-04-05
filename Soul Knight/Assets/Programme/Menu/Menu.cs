using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Menu : MonoBehaviour
{
    public GameObject pauseMenu;
    public AudioMixer mainMixer;
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            if (pauseMenu.activeSelf) ResumeGame();
            else PauseGame();
    }
    public void PlayGame()//怎么调用？
    {
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void VolumeControl(float volume)
    {
        volume = -volume*volume / 125;
        mainMixer.SetFloat("MainVolume",volume); 
    }
    public void Restart()
    {
        Invoke(nameof(Restart_), 3);
    }
    public void Restart_()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
