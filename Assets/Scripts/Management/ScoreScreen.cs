using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ScoreScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTimeText;
    [SerializeField] private TextMeshProUGUI parTimeText;

    private GameManager gm;

    void Start()
    {
        gm = GameObject.FindObjectOfType<GameManager>();

        // Show time spent in level
        if (gm.underPar) {
            levelTimeText.color = Color.green;
            parTimeText.color = Color.green;
        }
        else {
            levelTimeText.color = Color.white;
            parTimeText.color = Color.red;
        }
        levelTimeText.text = FloatToStringTime(gm.prevLevelTime);
        parTimeText.text = FloatToStringTime(gm.prevParTime);
    }

    private string FloatToStringTime(float time) {
        string min = $"{Mathf.Floor(time / 60f):00}";
        string sec = $"{Mathf.Floor(time % 60f):00}";

        return min + ":" + sec;
    }

    public void ToNextLevel() {
        gm.ToNextLevel();
    }

    public void ToMainMenu() {
        Destroy(gm);
        SceneManager.LoadScene("Main Menu");
    }
}
