using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UILogger : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI debugger;

    public void ReplaceLog(string txt) => debugger.text = txt;
    public void PushLog(string appendedText) => debugger.text += appendedText;
}
