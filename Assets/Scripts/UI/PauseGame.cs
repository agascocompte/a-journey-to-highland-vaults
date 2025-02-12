using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
	public static bool GameIsPaused = false;
    public GameObject pauseMenu;

    void Update()
	{
		if (Input.GetKeyDown("p"))
        {
			if (GameIsPaused)
            {
                Resume();              
            } 
            else
            {
                Pause();
            }
        }

        if (Input.GetKeyDown("r"))
        {
            if (GameIsPaused)
            {
                Resume();
                MapGenerator.ReturnToMainManuFromDeath();                
            }           
        }
    }

    void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        GameIsPaused = true;
    }
}
