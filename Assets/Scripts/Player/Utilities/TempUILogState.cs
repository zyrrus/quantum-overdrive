using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TempUILogState : MonoBehaviour
{
    [SerializeField] Text text;
    private PlayerStateMachine psm;

    private void Awake() => psm = GetComponent<PlayerStateMachine>();
    private void Update()
    {
        string state = psm.CurrentState.Name();
        text.text = state;
    }
}
