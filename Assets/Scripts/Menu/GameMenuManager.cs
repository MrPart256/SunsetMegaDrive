using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameMenuManager : MonoBehaviour
{
    public void Online()
    {
        SceneManager.LoadScene(2);
    }
    public void SinglePlayer()
    {
       // SceneManager.LoadScene();
    }
    public void Options()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
