using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonScripts : MonoBehaviour
{
    // Public functions
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ControlsMenu(int menuNumber)
    {
        switch (menuNumber)
        {
            case 0:
                SceneManager.LoadScene("Controls-0");
                break;
            case 1:
                SceneManager.LoadScene("Controls-1");
                break;
            case 2:
                SceneManager.LoadScene("Controls-2");
                break;
            case 3:
                SceneManager.LoadScene("Controls-3");
                break;
            case 4:
                SceneManager.LoadScene("Controls-4");
                break;
            default:
                throw new System.Exception("No condition set for int " + menuNumber);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartLevel()
    {
        
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1f;
        print(Time.timeScale);
    }
}
