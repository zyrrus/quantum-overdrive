using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthRing : MonoBehaviour
{
    private Image healthRing;
    private float amount;
    private float lerpSpeed;

    private void Awake() => healthRing = GetComponent<Image>();
    private void Start() => UpdateFill(1);
    private void Update()
    {
        lerpSpeed = 3f * Time.deltaTime;
        UpdateFill(amount);
    }

    public void UpdateFill(float amount)
    {
        this.amount = amount;
        healthRing.fillAmount = Mathf.Lerp(healthRing.fillAmount, amount, lerpSpeed);
        ChangeColor();
    }

    public void ChangeColor()
    {
        Color healthColor = Color.Lerp(Color.red, Color.green, amount);
        healthRing.color = healthColor;
    }
}
