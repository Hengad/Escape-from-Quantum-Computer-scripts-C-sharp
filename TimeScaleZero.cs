using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaleZero : MonoBehaviour
{
    private void OnEnable()
    {
        Time.timeScale = 0.0f;
    }
}
