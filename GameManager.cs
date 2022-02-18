using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject player;
    public GameObject HUD;
    public GameObject eventSystem;
    public GameObject vignette;
    public GameObject deathScreenCanvas;
    public GameObject winScreenCanvas;

    public Transform pfBullet;
    public Transform droppedWeapon;

    public List<GameObject> entanglementTaggedEnemies = new List<GameObject>();
    public bool playerTriesToEnterGate = false;
    HudController hudController;
    public bool[] levelsLoaded;
    int globalTargetIndex;

    private void Start()
    {
        levelsLoaded = new bool[20];
        for (int i = 0; i < 20; i++)
            levelsLoaded[i] = false;

        DontDestroyOnLoad(gameObject); //self
        DontDestroyOnLoad(mainCamera);
        DontDestroyOnLoad(player);
        DontDestroyOnLoad(HUD);
        DontDestroyOnLoad(eventSystem);
        DontDestroyOnLoad(vignette);
        DontDestroyOnLoad(deathScreenCanvas);
        DontDestroyOnLoad(winScreenCanvas);

        //SceneManager.LoadScene(2);
        SceneTransition(1, 2);
        player.transform.position = new Vector3(7.5f, -0.5f, 0);
    }

    public void ChangeLevel(Vector3 playerPosition)
    {
        int activeScene = SceneManager.GetActiveScene().buildIndex;

        if (playerPosition.y > 3.75f || playerPosition.y < -3.75f || playerPosition.x > 7.75f || playerPosition.x < -7.75f)
        {
            player.GetComponent<PlayerController>().speed = 0.8f;
        }

        if (playerTriesToEnterGate)
        {
            playerTriesToEnterGate = false;
            if (GameObject.FindGameObjectWithTag("LevelObject") == null || GameObject.FindGameObjectWithTag("LevelObject").transform.childCount == 0)
            {
                GameObject[] gates = GameObject.FindGameObjectsWithTag("Gate");
                for (int i = 0; i < gates.Length; i++)
                {
                    gates[i].GetComponent<CompositeCollider2D>().isTrigger = true;
                }
            }
        }

        //TOP
        if (playerPosition.y > 4.5f)
        {
            SceneTransition(activeScene, activeScene - 5);
            player.transform.position = new Vector3(0.5f, -3.5f, 0);
        }
        //DOWN
        else if (playerPosition.y < -4.5f)
        {
            SceneTransition(activeScene, activeScene + 5);
            player.transform.position = new Vector3(0.5f, 3.5f, 0);
        }
        //RIGHT
        else if (playerPosition.x > 8.5f)
        {
            SceneTransition(activeScene, activeScene + 1);
            player.transform.position = new Vector3(-7.5f, -0.5f, 0);
        }
        //LEFT
        else if (playerPosition.x < -8.5f)
        {
            SceneTransition(activeScene, activeScene - 1);
            player.transform.position = new Vector3(7.5f, -0.5f, 0);
        }
    }

    public void SceneTransition(int startIndex, int targetIndex)
    {
        if (targetIndex == 22)
        {
            winScreenCanvas.SetActive(true);
            Time.timeScale = 0.0f;
            return;
        }

        globalTargetIndex = targetIndex;

        SceneManager.LoadScene(targetIndex);
        player.GetComponent<PlayerController>().speed = 4.0f;

        hudController = GameObject.FindGameObjectWithTag("HUD").GetComponent<HudController>();
        hudController.SceneTransition(targetIndex);
    }

    public void TestiHomma()
    {
        GameObject.FindGameObjectWithTag("LevelObject").gameObject.SetActive(true);
    }

    public void BackToMainMenu()
    {
        Destroy(player);
        Destroy(HUD);
        Destroy(eventSystem);
        Destroy(vignette);
        Destroy(deathScreenCanvas);
        Destroy(winScreenCanvas);
        Destroy(mainCamera);
        Destroy(gameObject); //self
        Destroy(GameObject.FindGameObjectWithTag("Music").gameObject);

        SceneManager.LoadScene(0);
        Time.timeScale = 1;
    }
}
