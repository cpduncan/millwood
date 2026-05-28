using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine.Rendering;


public class WeaponSlot : MonoBehaviour
{

    // [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform cameraTransform;

    private GameObject model;

    private string equippedWeapon = "";

    int bulletsLeft;
    bool shooting, readyToShoot, reloading;

    private float spread, range, reloadTimeSeconds;
    private int damage, magazineSize, ammoValue;
    
    private bool allowButtonHold, burst;
    private float shotCooldownSeconds, burstCooldownSeconds;
    private int bulletsPerShot; 

    public Camera fpsCam;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    
    [SerializeField] private PlayerDataController playerData;
    public TextMeshProUGUI text;


    public void Equip(WeaponData weaponData)
    {
        Destroy(model);
        InitWeapon(weaponData);
    }

    private void Awake()
    {
        readyToShoot = true;
    }

    private void InitWeapon(WeaponData weaponData)
    {
        equippedWeapon = weaponData.name;
        
        bulletsLeft = weaponData.magazineSize;
        
        model = Instantiate(weaponData.model, cameraTransform);
        model.transform.localPosition = weaponData.modelOffsetPosition;
        model.transform.localRotation = weaponData.modelOffsetRotation;
        model.transform.localScale = Vector3.one * weaponData.modelOffsetScale;
        model.GetComponentInChildren<MeshRenderer>().material = weaponData.material;

        spread = weaponData.spread;
        range = weaponData.range;
        damage = weaponData.damage;
        magazineSize = weaponData.magazineSize;
        ammoValue = weaponData.ammoValue;
        reloadTimeSeconds = weaponData.reloadTimeSeconds;
        
        allowButtonHold = weaponData.allowButtonHold;
        shotCooldownSeconds = weaponData.shotCooldownSeconds;
        burst = weaponData.burst;
        burstCooldownSeconds = weaponData.burstCooldownSeconds;
        bulletsPerShot = weaponData.bulletsPerShot;
        
        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    public void Shoot()
    {
        if (equippedWeapon.Equals(""))
            return;

        shooting = true;
    }
    
    public void ShootStop()
    {
        shooting = false;
    }
    
    private void Update()
    {
        if (shooting && readyToShoot && bulletsLeft > 0 && !reloading)
        {
            readyToShoot = false;

            for (int i = 0; i < bulletsPerShot; i++)
            {
                float x = Random.Range(-spread, spread);
                float y = Random.Range(-spread, spread);
        
                Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);
        
                if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
                {
                    rayHit.collider.SendMessageUpwards("Damage", damage);
                    GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = rayHit.point; 
                }
            } 
        
            bulletsLeft--;
            text.SetText(bulletsLeft + " / " + magazineSize);
        
            Invoke(nameof(ResetShot), shotCooldownSeconds);
        }
    } 
    
    private void ResetShot()
    {
        readyToShoot = true;
    }
    
    public void Reload()
    {
        if (equippedWeapon.Equals(""))
            return;

        if (playerData.GetAmmo() == 0) return;
        
        shooting = false;
        reloading = true;
        Invoke("ReloadFinished", reloadTimeSeconds);
    }
    
    private void ReloadFinished()
    {
        
        playerData.ChangeAmmo(-magazineSize);
        
        int ammo = playerData.GetAmmo();
        if (ammo < 0)
        {
            bulletsLeft = ammo + magazineSize;
            playerData.SetAmmo(0);
        }
        else
        {
            bulletsLeft = magazineSize;
        }
        
        reloading = false;
        
        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    public int Mill()
    {
        if (equippedWeapon.Equals("") || shooting)
            return 0;
        
        Destroy(model);
        equippedWeapon = "";
        return ammoValue;
    }
}
