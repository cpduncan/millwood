using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class PlayerDataController : MonoBehaviour
{

    [Header("Player Data")]
    [SerializeField] public Transform _transform;
    [SerializeField] private int ammo;
    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    
    [Header("UI Objects")]
    [SerializeField] private TextMeshProUGUI uiAmmoText;
    [SerializeField] private TextMeshProUGUI uiHealthText;
    [SerializeField] private TextMeshProUGUI uiMagText;
    
    public int GetAmmo() { return ammo; }

    private void Awake()
    {
        ChangeAmmo(0);
        ChangeHealth(0);
    }

    public void ChangeAmmo(int add)
    {
        ammo += add;
        uiAmmoText.text = ammo.ToString();
    }

    public void SetAmmo(int set)
    {
        ammo = set;
        uiAmmoText.text = ammo.ToString();
    }

    public void SetMagText(String set)
    {
        uiMagText.text = set;
    }

    public void SetHealth(int set)
    {
        health += set;
        if  (health > maxHealth) health = maxHealth;
        uiHealthText.text = health.ToString() + "/" + maxHealth.ToString();
    }

    public void SetHealth(int set, int setMax)
    {
        health += set;
        maxHealth = setMax;
        if  (health > maxHealth) health = maxHealth;
        uiHealthText.text = health.ToString() + "/" + maxHealth.ToString();
    }

    public void ChangeHealth(int add)
    {
        health += add;
        if  (health > maxHealth) health = maxHealth;
        uiHealthText.text = health.ToString() + "/" + maxHealth.ToString();
    }

    public void ChangeHealth(int add, int setMax)
    {
        health += add;
        maxHealth = setMax;
        if  (health > maxHealth) health = maxHealth;
        uiHealthText.text = health.ToString() + "/" + maxHealth.ToString();
    }

}
