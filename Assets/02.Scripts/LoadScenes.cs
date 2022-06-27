using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadScenes : MonoBehaviour
{
    public void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    public void LoadGameClear()
    {
        SceneManager.LoadScene("GameClear");
    }
    public void LoadMain()
    {
        SceneManager.LoadScene("Main");
    }
    public void LoadTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");
    }
    public void Exit()
    {
        Application.Quit();
    }
}
