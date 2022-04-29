using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
    private float timerLength = 0;
    private float currentTime = 0;

    public bool isOver { get => currentTime <= 0; }

    public Timer()
    {
        timerLength = 0;
        currentTime = 0;
    }

    public Timer(float duration)
    {
        timerLength = duration;
        currentTime = 0;
    }

    public void Reset() => currentTime = timerLength;
    public void Reset(float newDuration)
    {
        timerLength = newDuration;
        Reset();
    }

    public void Tick() { if (currentTime > 0) currentTime -= Time.deltaTime; }

    public override string ToString() => currentTime + "/" + timerLength;
}