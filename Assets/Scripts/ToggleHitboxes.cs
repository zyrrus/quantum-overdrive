using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum AttackDirection { Forward, Up, Down }

public class ToggleHitboxes : MonoBehaviour
{
    [SerializeField] private GameObject forwardHitbox;
    [SerializeField] private GameObject upHitbox;
    [SerializeField] private GameObject downHitbox;

    private GameObject activeHitbox;

    private void Start()
    {
        forwardHitbox.SetActive(false);
        upHitbox.SetActive(false);
        downHitbox.SetActive(false);
    }

    public void EnableHitbox(AttackDirection attack, float damage)
    {
        switch (attack)
        {
            case AttackDirection.Forward:
                activeHitbox = forwardHitbox;
                break;
            case AttackDirection.Up:
                activeHitbox = upHitbox;
                break;
            case AttackDirection.Down:
                activeHitbox = downHitbox;
                break;
        }

        activeHitbox.GetComponent<AttackCollider>().SetDamage(damage);
        activeHitbox.SetActive(true);
    }

    public void DisableHitbox()
    {
        if (activeHitbox == null) return;

        activeHitbox.SetActive(false);
        activeHitbox = null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        if (activeHitbox != null)
            Gizmos.DrawWireSphere(activeHitbox.transform.position, 0.5f);
    }
}
