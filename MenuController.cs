using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public HudController hudController;

    public void Start()
    {
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("Music"));
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("SoundController").gameObject);
    }

    public void Play()
    {
        SceneManager.LoadScene(1); //load setup level (index 1 = level0)
    }

    public void Exit()
    {
        Application.Quit();
    }
}
