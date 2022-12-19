using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Map1");
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("BeforeGame");
    }

    public void Didacticiel()
    {
        SceneManager.LoadScene("Didacticiel");
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }   
}
