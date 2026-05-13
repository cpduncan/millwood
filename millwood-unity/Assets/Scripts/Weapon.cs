using UnityEngine;
using TMPro;


public class Weapon : MonoBehaviour
{
    
    //Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;


    //bools 
    bool shooting, readyToShoot, reloading;


    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;
    
    [SerializeField] private PlayerDataController playerData;


    //Graphics
    public TextMeshProUGUI text;


    private void Awake()
    {
        bulletsLeft = magazineSize;
    }
    public void Shoot()
    {
        if (bulletsLeft == 0) return;
        

        //Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);


        //Calculate Direction with Spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);


        //RayCast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(Time.timeAsRational + ": " + rayHit.collider.name);


            if (rayHit.collider.CompareTag("Shootable"))
            {
                Debug.Log(Time.timeAsRational +": Shot a Shootable!");
                rayHit.collider.SendMessageUpwards("Damage", damage);
            }
        }

        bulletsLeft--;
        bulletsShot--;
        text.SetText(bulletsLeft + " / " + magazineSize);
    }
    public void Reload()
    {
        if (playerData.getAmmo() == 0) return;
        
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    private void ReloadFinished()
    {
        playerData.ChangeAmmo(-magazineSize);
        
        int ammo = playerData.getAmmo();
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
}
