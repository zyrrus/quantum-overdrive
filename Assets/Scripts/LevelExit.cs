using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelExit : MonoBehaviour
{
    [SerializeField] private LevelManager lm;


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Player") {
            lm.ExitLevel();
        }
    }
}
