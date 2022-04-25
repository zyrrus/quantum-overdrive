using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static void OnResume()
    {
        Debug.Log("Resume");
    }

    public static void OnOptions()
    {
        Debug.Log("Options");
    }

    public static void OnQuit()
    {
        SceneManager.LoadScene(0);
    }
}