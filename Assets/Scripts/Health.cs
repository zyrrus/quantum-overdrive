using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private HealthRing health;

    [SerializeField] protected float maxHealth;
    protected float curHealth;

    private void Start()
    {
        curHealth = maxHealth;
        LogHealth();
    }

    public void LoseHealth(float amount)
    {
        curHealth = Mathf.Clamp(curHealth - amount, 0, maxHealth);
        LogHealth();
        DestroyOnDeath();
    }
    public void GainHealth(float amount)
    {
        curHealth = Mathf.Clamp(curHealth + amount, 0, maxHealth);
        LogHealth();
    }

    public bool IsDead() => curHealth == 0;

    private void LogHealth() => health.UpdateFill(curHealth / maxHealth);

    private void DestroyOnDeath()
    {
        if (IsDead())
        {
            if (gameObject.tag != "Player")
            {
                Destroy(gameObject);
            }

        }
    }
}
