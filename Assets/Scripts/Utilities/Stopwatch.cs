using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stopwatch
{
    private float currentTime = 0;
    private bool running = false;

    public Stopwatch() {
        currentTime = 0;
        running = false;
    }

    public void Start() => running = true;
    public void Stop() => running = false;

    public void Tick() { if (running) currentTime += Time.deltaTime; }
    public void Reset() => currentTime = 0;

    public float GetTime() => currentTime;

    public override string ToString() {
        string min = $"{Mathf.Floor(currentTime / 60f):00}";
        string sec = $"{Mathf.Floor(currentTime % 60f):00}";

        return min + ":" + sec;
    }
}
