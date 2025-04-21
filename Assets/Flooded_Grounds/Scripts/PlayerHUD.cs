using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public Image healthBarFill;
    public Image staminaBarFill;

    public void UpdateHealth(float currentHealth, float maxHealth)
    {
        healthBarFill.fillAmount = currentHealth / maxHealth;
    }

    public void UpdateStamina(float currentStamina, float maxStamina)
    {
        staminaBarFill.fillAmount = currentStamina / maxStamina;
    }

}
