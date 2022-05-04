using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void ToMainMenu() {
        GameManager gm = GameObject.FindObjectOfType<GameManager>();
        Destroy(gm);
        SceneManager.LoadScene("Main Menu");
    }
}
