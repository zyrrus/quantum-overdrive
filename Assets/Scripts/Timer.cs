using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float timerLength = 0;
    private float currentTime = 0;

    public bool isOver { get => currentTime <= 0 && currentTime < timerLength; }

    public Timer(float duration)
    {
        timerLength = duration;
        currentTime = duration;
    }

    public void Tick() => currentTime -= Time.deltaTime;
    public void Reset() => currentTime = timerLength;
}
