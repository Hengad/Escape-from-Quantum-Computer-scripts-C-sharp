using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SetActive : MonoBehaviour
{
    GameManager gameManager;
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if(gameManager.levelsLoaded[SceneManager.GetActiveScene().buildIndex - 2])
        {
            GameObject.FindGameObjectWithTag("LevelObject").gameObject.SetActive(false);
        }

        else
        {
            gameManager.levelsLoaded[SceneManager.GetActiveScene().buildIndex - 2] = true;
        }
    }
}
