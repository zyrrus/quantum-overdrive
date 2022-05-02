using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private CheckPoint prev;
    public bool isNew { get; private set; } = true;

    public void DisableCheckPoint() {
        // Disable older checkpoints (if skipped)
        if (prev != null) prev.DisableCheckPoint();
        isNew = false;
        // TODO: Sprite change
    }
}
