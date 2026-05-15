using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Scriptable Objects/WeaponData")]
public class WeaponData : ScriptableObject
{

    [Header("Behavior")]
    [SerializeField] public int magazineSize;
    [SerializeField] public int damage;
    [SerializeField] public float spread;
    [SerializeField] public float range;

    [SerializeField] public float reloadTimeSeconds;
    // [SerializeField] private float shotCooldownSeconds;


    // [SerializeField] private bool burst;
    // [SerializeField] private float burstLength;
    // [SerializeField] private float burstCooldownSeconds;


    // [SerializeField] private int bulletsPerShot;


    // [SerializeField] private bool allowButtonHold;


    [Header("Model")]
    [SerializeField] public GameObject model;
    [SerializeField] public Vector3 modelOffsetPosition;
    [SerializeField] public Quaternion modelOffsetRotation;
    [SerializeField] public float modelOffsetScale;
    [SerializeField] public Material material;

    [Header("Milling")]
    [SerializeField] public int ammoValue;

}