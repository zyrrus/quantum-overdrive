using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public bool underPar { get; private set; }
    private float prevLevelTime;
    private float prevParTime;


    public void ToScoreScreen(float levelTime, float parTime) {
        prevLevelTime = levelTime;
        prevParTime = parTime;
        underPar = levelTime <= parTime;
    }
}
