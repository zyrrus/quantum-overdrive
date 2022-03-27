using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private UILogger logger;

    [SerializeField] private float maxHealth;
    private float curHealth;

    private void Awake()
    {
        logger = GetComponent<UILogger>();
    }

    private void Start()
    {
        curHealth = maxHealth;
        LogHealth();
    }

    public void LoseHealth(float amount)
    {
        curHealth = Mathf.Clamp(curHealth - amount, 0, maxHealth);
        LogHealth();
    }
    public void GainHealth(float amount)
    {
        curHealth = Mathf.Clamp(curHealth + amount, 0, maxHealth);
        LogHealth();
    }

    public bool IsDead() => curHealth == 0;

    private void LogHealth() => logger.ReplaceLog($"{curHealth} / {maxHealth}");
}
