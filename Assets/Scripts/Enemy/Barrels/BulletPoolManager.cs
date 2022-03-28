using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Resets static bullet pool setup variable
public class BulletPoolManager : MonoBehaviour
{
    private void Awake() {
        BarrelBasic.bulletsSetup = false;
        Destroy(gameObject);
    }
}
