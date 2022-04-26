using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropsKey : MonoBehaviour
{
    [SerializeField] private GameVariablesController gvc;
    [SerializeField] private int keyNum;

    private void OnDisable() {
        gvc.keys[keyNum] = true;
    }
}
