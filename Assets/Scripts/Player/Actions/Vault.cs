using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vault : MonoBehaviour
{
    private Rigidbody2D rb;
    private PlayerCore pc;

    [SerializeField] private Flag lowerVaultFlag;
    [SerializeField] private Flag upperVaultFlag;

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

        bool canVault = !isVaulting && pc.isGrounded && !upperVaultFlag.IsTriggered() && lowerVaultFlag.IsTriggered();

        if (canVault)
        {
            isVaulting = true;
            float force = vaultForce.y - rb.velocity.y;
            rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        }

        if (isVaulting && !lowerVaultFlag.IsTriggered())
        {
            isVaulting = false;
            float force = vaultForce.x;
            rb.AddForce(new Vector2(pc.isFacingRight ? 1 : -1, 0) * force, ForceMode2D.Impulse);
        }
    }
}
