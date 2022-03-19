using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerCore pc;

    [SerializeField] private Vector2 vaultForce;
    private bool isVaulting;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerCore>();
    }

    void Update()
    {
        if (pc.isGrounded) isVaulting = false;

        bool canVault = !isVaulting && pc.isGrounded && !pc.isHittingUpperWall && pc.isHittingLowerWall;

        if (canVault)
        {
            isVaulting = true;
            float force = vaultForce.y - rb.velocity.y;
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        if (isVaulting && !pc.isHittingLowerWall)
        {
            isVaulting = false;
            float force = vaultForce.x;
            rb.AddForce(new Vector2(pc.isFacingRight ? 1 : -1, 0) * force, ForceMode2D.Impulse);
        }
    }
}
