using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameVariablesController : MonoBehaviour
{
    public bool[] keys;

    public bool HasAllKeys() {
        foreach (bool key in keys) {
            if (!key) return false;
        }
        return true;
    }

    public int NumKeysLeft() {
        int num = 0;
        foreach (bool key in keys) {
            if (!key) num++;
        }
        return num;
    }
}
