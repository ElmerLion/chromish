using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour {

    private Transform firePoint;
    private WeaponSO equippedWeapon;
    private ParticleSystem muzzleFlash;
    private Transform bulletPrefab;
    private Transform raycastPoint;

    private bool isReloading;
    private float timeSinceLastShot;
    private int currentAmmo;
    public bool playerInRange;
    public float shootRange;
    public float backupDistance;

    private Enemy enemy;

    private void Awake() {
        //equippedWeapon = transform.Find("M48").GetComponent<WeaponTypeHolder>().weaponSO;
        //Debug.Log(equippedWeapon.nameString);
    }

    private void Start() {
        shootRange = 30;
        backupDistance = 20;
        
        muzzleFlash = Resources.Load<ParticleSystem>("Flash");
        bulletPrefab = Resources.Load<Transform>("Bullet");

        Transform weapon = Utils.FindChildRecursively(transform, "M48");
        equippedWeapon = weapon.GetComponent<WeaponTypeHolder>().weaponSO;

        firePoint = weapon.GetChild(5);
        raycastPoint = weapon.GetChild(6);

        enemy = transform.GetComponent<Enemy>();

        currentAmmo = equippedWeapon.magazineSize;
        isReloading = false;
    }

    private void Update() {
        if (!enemy.isDead) {
            timeSinceLastShot += Time.deltaTime;
            HandleShooting();
        }
        
    }

    private void HandleShooting() {
        Vector3 aimDir = (enemy.GetTarget() - firePoint.position).normalized;
        if (currentAmmo <= 0) {
            StartReload();
        }

        if (Physics.Raycast(raycastPoint.position, aimDir, out RaycastHit hit, shootRange)) {
            Player player = hit.transform.GetComponent<Player>();
            if (!isReloading && currentAmmo > 0 && CanShoot() && playerInRange && player != null) {

                Quaternion bulletRotation = Quaternion.LookRotation(aimDir, Vector3.up);

                currentAmmo--;
                timeSinceLastShot = 0f;

                GameObject bullet = Instantiate(bulletPrefab.gameObject, firePoint.position, bulletRotation);
                bullet.GetComponent<Bullet>().SetWeapon(equippedWeapon);

                Instantiate(muzzleFlash.gameObject, firePoint);
            }
        }

        
    }

    public void StartReload() {
        if (!isReloading) {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload() {
        isReloading = true;

        yield return new WaitForSeconds(equippedWeapon.reloadTime);
        currentAmmo = equippedWeapon.magazineSize;

        isReloading = false;

    }

    private bool CanShoot() {
        return !isReloading && timeSinceLastShot > 1f / (equippedWeapon.fireRate / 60f);
    }

}
