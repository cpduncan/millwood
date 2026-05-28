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

    [SerializeField] public bool allowButtonHold;
    [SerializeField] public float shotCooldownSeconds; // (time between each shot)
    [SerializeField] public bool burst;
    [SerializeField] public float burstCooldownSeconds; // (between bursts)
    [SerializeField] public int bulletsPerShot; // (for shotguns)


    [Header("Model")]
    [SerializeField] public GameObject model;
    [SerializeField] public Vector3 modelOffsetPosition;
    [SerializeField] public Quaternion modelOffsetRotation;
    [SerializeField] public float modelOffsetScale;
    [SerializeField] public Material material;

    [Header("Milling")]
    [SerializeField] public int ammoValue;

}