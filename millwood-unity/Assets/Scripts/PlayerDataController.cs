using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerDataController : MonoBehaviour
{

    [Header("Player Data")]
    [SerializeField] private int ammo;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    
    [Header("Reference Objects")]
    [SerializeField] private TextMeshProUGUI uiAmmoText;
    [SerializeField] private TextMeshProUGUI uiHealthText;

    public void updateAmmo(int add)
    {
        ammo += add;
        uiAmmoText.text = ammo.ToString();
    }

    public void updateHealth(int add)
    {
        health += add;
        if  (health > maxHealth) health = maxHealth;
        uiHealthText.text = health.ToString() + "/" + maxHealth.ToString();
    }

    public void updateHealth(int add, int setMax)
    {
        health += add;
        maxHealth = setMax;
        if  (health > maxHealth) health = maxHealth;
        uiHealthText.text = health.ToString() + "/" + maxHealth.ToString();
    }

}
