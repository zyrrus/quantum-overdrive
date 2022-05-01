using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelManager : MonoBehaviour
{
    // Other objects
    [SerializeField] private GameObject startDoor;
    [SerializeField] private Text timeText;
    [SerializeField] private ObstacleBase[] extraObstacles;
    private ObstacleBase[] allObstacles;
    private GameManager gm;

    // Level specific variables
    [SerializeField] private ParTime parTimeStruct;
    private float parTime;

    [Serializable]
    struct ParTime {
        public int min;
        public int sec;
    }
    private float ParTimeToFloat(ParTime time) {
        return (float) time.min * 60 + time.sec;
    }



    [SerializeField] private float startTime; 
    private Stopwatch levelTime;
    
    private void Start() {
        levelTime = new Stopwatch();
        parTime = ParTimeToFloat(parTimeStruct);

        Invoke("OpenStart", startTime);

        allObstacles = GameObject.FindObjectsOfType<ObstacleBase>();

        // Level has less obstacles if last level was completed under par
        gm = GameObject.FindObjectOfType<GameManager>();
        if (gm.underPar) {
            foreach (ObstacleBase obj in extraObstacles) {
                obj.gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        levelTime.Tick();
        UpdateTime();
    }

    private void OpenStart() {
        startDoor.SetActive(false);
        levelTime.Start();
    }

    private void UpdateTime() {
        timeText.text = levelTime.ToString();
    }

    public void ExitLevel() {
        levelTime.Stop();
        gm.ToScoreScreen(levelTime.GetTime(), parTime);
    }

    public void ResetLevel() {
        foreach (ObstacleBase obj in allObstacles) {
            obj.ResetObstacle();
        }
    }
}
