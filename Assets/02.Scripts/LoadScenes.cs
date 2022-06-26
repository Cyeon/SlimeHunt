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
        Debug.Log("t");
        SceneManager.LoadScene("Title");
    }
    public void Exit()
    {
        Debug.Log("q");
        Application.Quit();
    }
}
