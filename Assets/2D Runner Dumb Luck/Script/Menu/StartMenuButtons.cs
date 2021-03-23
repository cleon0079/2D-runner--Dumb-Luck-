using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using TMPro;

public class StartMenuButtons : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("GameScene");

    }
    
    public void Options()
    {

    }

    public void Credits()
    {

    }

    public void Exit()
    {
        Debug.Log("Exit Game");
        Application.Quit();
    }
}
