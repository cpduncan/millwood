using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Item_Weapon : MonoBehaviour
{

    [SerializeField] float hitboxSize;
    [SerializeField] WeaponData weaponData;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 1);
        Handles.Label(transform.position, weaponData.name);
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponentInChildren<WeaponSlot>().Equip(weaponData);
        Destroy(gameObject);
    }
}
