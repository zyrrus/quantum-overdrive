using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerLog : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private TextMeshProUGUI debugger;
    private int counter = 0;


    private void Start()
    {
        debugger = canvas.GetComponentInChildren<TextMeshProUGUI>();
        debugger.text = "Hello from script";
    }

    public void Log(string text) => debugger.text = text;
    public void PushLog(string appendedText) => debugger.text += appendedText;

}
