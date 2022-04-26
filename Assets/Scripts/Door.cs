using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    [SerializeField] private GameVariablesController gvc;
    [SerializeField] private TextMeshProUGUI requirementText;

    private Timer textTimer;
    [SerializeField] private float textFadeTime;

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") {
            if (gvc.HasAllKeys()) {
                Destroy(gameObject);
            }
            else {
                requirementText.enabled = true;
                int numKeys = gvc.NumKeysLeft();
                requirementText.text = numKeys.ToString();
                if (numKeys == 1) requirementText.text += " key left!";
                else requirementText.text += " keys left!";
                textTimer.Reset();
            }
        }
    }

    private void Awake() {
        textTimer = new Timer(textFadeTime);

        requirementText.enabled = false;
    }

    private void Update() {
        if (!textTimer.isOver) {
            textTimer.Tick();
        }
        else {
            requirementText.enabled = false;
        }
    }
}
