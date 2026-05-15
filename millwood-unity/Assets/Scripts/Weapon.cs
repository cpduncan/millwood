using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEditor.VersionControl;


public class Weapon : MonoBehaviour
{

    [SerializeField] private WeaponData _weaponData;
    [SerializeField] private Transform cameraTransform;

    private GameObject model;

    int bulletsLeft;
    bool shooting, readyToShoot, reloading;

    public Camera fpsCam;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    
    [SerializeField] private PlayerDataController playerData;
    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = _weaponData.magazineSize;
        model = Instantiate(_weaponData.model, cameraTransform);
        model.transform.position += _weaponData.modelOffsetPosition;
        model.transform.rotation *= _weaponData.modelOffsetRotation;
        model.transform.localScale *= _weaponData.modelOffsetScale;
        model.GetComponentInChildren<MeshRenderer>().material = _weaponData.material;
        text.SetText(bulletsLeft + " / " + _weaponData.magazineSize);
    }
    
    public void Shoot()
    {
        if (bulletsLeft == 0) return;
        

        //Spread
        float x = Random.Range(-_weaponData.spread, _weaponData.spread);
        float y = Random.Range(-_weaponData.spread, _weaponData.spread);


        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);


        //RayCast
        Debug.Log("Bang!");
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, _weaponData.range, whatIsEnemy))
        {
            Debug.Log(Time.timeAsRational + ": " + rayHit.collider.name);

            // if (rayHit.collider.CompareTag("Shootable"))
            // {
                Debug.Log(Time.timeAsRational +": Shot a Shootable!");
                rayHit.collider.SendMessageUpwards("Damage", _weaponData.damage);
            // }
        }

        bulletsLeft--;
        text.SetText(bulletsLeft + " / " + _weaponData.magazineSize);
    }
    
    public void Reload()
    {
        if (playerData.GetAmmo() == 0) return;
        
        reloading = true;
        Invoke("ReloadFinished", _weaponData.reloadTimeSeconds);
    }
    
    private void ReloadFinished()
    {
        playerData.ChangeAmmo(-_weaponData.magazineSize);
        
        int ammo = playerData.GetAmmo();
        if (ammo < 0)
        {
            bulletsLeft = ammo + _weaponData.magazineSize;
            playerData.SetAmmo(0);
        }
        else
        {
            bulletsLeft = _weaponData.magazineSize;
        }
        
        reloading = false;
        
        text.SetText(bulletsLeft + " / " + _weaponData.magazineSize);
    }

    public int Mill()
    {
        Destroy(model);
        Destroy(_weaponData);
        return _weaponData.ammoValue;
    }
}
