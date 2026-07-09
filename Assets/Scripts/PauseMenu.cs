using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public MenuMusic menuMusic;
public GameObject pauseMenu;
public GameObject settingsMenu;

private bool isPaused = false;

private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
public void ResumeGame()
    {
     pauseMenu.SetActive(false); 
     settingsMenu.SetActive(false); 

     Time.timeScale = 1;
     isPaused = false;    

     menuMusic.StopMusic();
    }

    public void PauseGame()
    {
     pauseMenu.SetActive(true); 
     settingsMenu.SetActive(false); 

     Time.timeScale = 0;
     isPaused = true;

     menuMusic.PlayMusic();

     Cursor.lockState = CursorLockMode.None;
     Cursor.visible = true;
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
}
