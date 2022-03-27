using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachineEnemyBasic : MonoBehaviour
{
    private delegate State State();
    private State state;

    public bool PlayerDetected { get; set; }
    public bool PlayerTracking { get; set; }
    public bool PlayerAttacking { get; set; }

    private bool inAttackAnimation;
    private bool inStaggerAnimation;
    private bool atHomeLocation;

    // private void Awake() {
    //     state = StateDetecting;
    // }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        state = StateDetecting;
        PlayerDetected = false;
        PlayerTracking = false;
        PlayerAttacking = false;
        inAttackAnimation = false;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        state = state();
        // Debug.Log("Detected =" + PlayerDetected);
        // Debug.Log("Tracking =" + PlayerTracking);
        // Debug.Log("Attacking=" + PlayerAttacking);
        // Debug.Log("AtkAnimat=" + inAttackAnimation);
    }

    // States
    private State StateDetecting() {
        // Debug.Log("Detecting");
        DetectAction();
        if (PlayerDetected) {
            return StateTracking;
        }
        else {
            return StateDetecting;
        }
    }

    private State StateTracking() {
        // Debug.Log("Tracking");
        TrackAction();
        if (PlayerAttacking) {
            return StateAttacking;
        }
        else if (PlayerTracking) {
            return StateTracking;
        }
        else {
            return StateResetting;
        }
    }

    private State StateAttacking() {
        // Debug.Log("Attacking");
        AttackAction();
        if (PlayerAttacking) {
            return StateAttackAnimation;
        }
        else {
            return StateTracking;
        }
    }

    private State StateStaggered() {
        // Debug.Log("Staggered");
        if (inStaggerAnimation) {
            return StateStaggered;
        }
        else {
            return StateTracking;
        }

    }

    private State StateResetting() {
        // Debug.Log("Resetting");
        ResetAction();
        if (atHomeLocation) {
            return StateDetecting;
        }
        else if (PlayerDetected) {
            return StateTracking;
        }
        else {
            return StateResetting;
        }
    }

    private State StateAttackAnimation() {
        // Debug.Log("AtkAnimation");
        if (inAttackAnimation) {
            return StateAttackAnimation;
        }
        else {
            return StateAttacking;
        }
    }

    // Actions - abstract methods
    public abstract void DetectAction();
    public abstract void TrackAction();
    public abstract void AttackAction();
    public abstract void ResetAction();
}
