using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveEnemiesFalse : MonoBehaviour
{
    GameManager gameManager;

    void SetEnemiesActiveFalse()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        gameManager.TestiHomma();
    }
}
