using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool underPar { get; private set; }
    public float prevLevelTime { get; private set; }
    public float prevParTime { get; private set; }
    private string nextLevelName;

    private void Awake() {
        underPar = false;
    }

    public void ToScoreScreen(float levelTime, float parTime, string nextLevel) {
        prevLevelTime = levelTime;
        prevParTime = parTime;
        underPar = levelTime <= parTime;
        nextLevelName = nextLevel;
        SceneManager.LoadScene("ScoreScreen");
    }

    public void ToNextLevel() {
        SceneManager.LoadScene(nextLevelName);
    }
}
