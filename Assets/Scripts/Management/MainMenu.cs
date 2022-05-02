using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject gmPrefab;
    [SerializeField] private string tutorialScene;


    public void OnNewGame()
    {
        GameObject gm = Instantiate(gmPrefab);
        DontDestroyOnLoad(gm);
        SceneManager.LoadScene(tutorialScene);
    }

    public void OnOptions()
    {
        Debug.Log("Options");
    }

    public void OnQuit()
    {
        Application.Quit();
    }
}
