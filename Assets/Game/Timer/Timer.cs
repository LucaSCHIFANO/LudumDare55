using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private bool isTimerRunning;
    private float currentTimer;

    private void Awake()
    {
        isTimerRunning = true;    
    }

    private void Update()
    {
        if (isTimerRunning)
        {
            currentTimer += Time.deltaTime;
        }
    }

    public string Stop()
    {
        isTimerRunning = false;
        
        int minutes = Mathf.FloorToInt(currentTimer / 60f);
        int seconds = Mathf.FloorToInt(currentTimer - minutes * 60);

        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
}
