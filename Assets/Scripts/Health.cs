using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private UILogger logger;

    [SerializeField] private float maxHealth;
    private float curHealth;

    private void Awake()
    {
        if (logger == null)
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
        DestroyOnDeath();
    }
    public void GainHealth(float amount)
    {
        curHealth = Mathf.Clamp(curHealth + amount, 0, maxHealth);
        LogHealth();
    }

    public bool IsDead() => curHealth == 0;

    private void LogHealth() => logger.ReplaceLog($"{curHealth} / {maxHealth}");

    private void DestroyOnDeath() {
        if (IsDead()) {
            if (gameObject.tag != "Player") {
                Destroy(gameObject);
            }

        }
    }
}
