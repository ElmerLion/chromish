using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class WeaponManager : MonoBehaviour {

    public static WeaponManager Instance { get; private set; }

    [Header("Essentials")]
    [SerializeField] private Transform bulletPrefab;
    [SerializeField] private LayerMask aimColliderMask = new LayerMask();
    [SerializeField] private Transform[] weapons;
    [SerializeField] private Transform crossHair;

    [Header("Ammo")]
    [SerializeField] private TextMeshProUGUI ammoCount;
    [SerializeField] private TextMeshProUGUI reloadingText;
    [SerializeField] private Transform hitmarker;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform weaponHolder;

    private Dictionary<Transform, int> weaponsCurrentAmmo;
    private Dictionary<Transform, float> weaponsTimeSinceLastShot;
    private Vector3 mouseShootPosition;
    private ParticleSystem muzzleFlash;
    private int currentWeaponIndex = 0;
    private float timeSinceLastShot;
    private WeaponSO activeWeaponSO;
    private bool isReloading;
    private bool isShooting;
    private Transform firePoint;
    private Transform muzzleFlashPoint;



    private AudioSource audioSource;
    private AudioClip activeClip;

    private void Awake() {
        Instance = this;

        weaponsCurrentAmmo = new Dictionary<Transform, int>();
        weaponsTimeSinceLastShot = new Dictionary<Transform, float>();
        isReloading = false;

        foreach (Transform weaponTransform in weapons) {
            weaponTransform.gameObject.SetActive(false);

            weaponsCurrentAmmo[weaponTransform] = weaponTransform.GetComponent<WeaponTypeHolder>().weaponSO.magazineSize;
            weaponsTimeSinceLastShot[weaponTransform] = 0;
        }

        
    }

    private void Start() {
        crossHair.position = new Vector2(Screen.width / 2, Screen.height / 2);

        currentWeaponIndex = 0;
        SwitchWeapon(currentWeaponIndex);

        GameInput.attackInputPressed += StartShooting;
        GameInput.attackInputReleased += StopShooting;
        GameInput.reloadInput += StartReload;

        muzzleFlash = Resources.Load<ParticleSystem>("Flash");

        hitmarker.gameObject.SetActive(false);

        UpdateAmmoCount();

    }

    private void OnDisable() {
        GameInput.attackInputPressed -= StartShooting;
        GameInput.attackInputReleased -= StopShooting;
        GameInput.reloadInput -= StartReload;
    }

    private void Update() {
        HandleWeaponSwitching();

        
        
        weaponsTimeSinceLastShot[weapons[currentWeaponIndex]] += Time.deltaTime;
        
        HandleShooting();

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        weaponHolder.rotation = Quaternion.LookRotation(ray.direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 999f)) {
            mouseShootPosition = hit.point;
        }
    }

    private void HandleWeaponSwitching() {
        if (!isReloading) {
            float scrollWheel = Input.GetAxis("Mouse ScrollWheel");
            if (scrollWheel != 0f) {
                int newIndex = currentWeaponIndex + (scrollWheel > 0f ? 1 : -1);

                newIndex = Mathf.Clamp(newIndex, 0, weapons.Length - 1);

                SwitchWeapon(newIndex);
            }
        }
    }

    private void StartShooting() {
        isShooting = true;
    }

    private void StopShooting() {
        isShooting = false;
    }

    private void HandleShooting() {
        if (isShooting) {
            if (weaponsCurrentAmmo[weapons[currentWeaponIndex]] <= 0) {
                StartReload();
            }
            if (CanShoot() && weaponsCurrentAmmo[weapons[currentWeaponIndex]] > 0) {
                if (firePoint != null && muzzleFlash != null) {
                    Vector3 aimDir = (mouseShootPosition - firePoint.position).normalized;

                    Quaternion bulletRotation = Quaternion.LookRotation(aimDir, Vector3.up);

                    weaponsCurrentAmmo[weapons[currentWeaponIndex]]--;

                    weaponsTimeSinceLastShot[weapons[currentWeaponIndex]] = 0;

                    UpdateAmmoCount();

                    CameraShakeEffect.Instance.ShakeCamera(1f, .1f);
                    GameObject bullet = Instantiate(bulletPrefab.gameObject, firePoint.position, bulletRotation);
                    
                    Bullet bulletScript = bullet.GetComponent<Bullet>();

                    bulletScript.SetWeapon(activeWeaponSO);
                    bulletScript.SetIsPlayer();

                    PlayActiveWeaponSound();

                    Instantiate(muzzleFlash.gameObject, muzzleFlashPoint);

                    
                    
                } else {
                    firePoint = weapons[currentWeaponIndex].Find("FirePoint");
                    muzzleFlashPoint = weapons[currentWeaponIndex].Find("MuzzleFlashPoint");
                }
            }
        }
    }

    private bool CanShoot() {
        return !isReloading && weaponsTimeSinceLastShot[weapons[currentWeaponIndex]] > 1f / (activeWeaponSO.fireRate / 60f);
    }

    private void SwitchWeapon(int newWeaponIndex) {
        weapons[currentWeaponIndex].gameObject.SetActive(false);

        currentWeaponIndex = newWeaponIndex;
        activeWeaponSO = weapons[currentWeaponIndex].transform.GetComponent<WeaponTypeHolder>().weaponSO;
        UpdateAmmoCount();


        firePoint = weapons[currentWeaponIndex].Find("FirePoint");
        muzzleFlashPoint = weapons[currentWeaponIndex].Find("MuzzleFlashPoint");

        weapons[currentWeaponIndex].gameObject.SetActive(true);
        audioSource = weapons[currentWeaponIndex].GetComponent<AudioSource>();
        activeClip = audioSource.clip;
    }

    public void StartReload() {
        if (!isReloading) {
            StartCoroutine(Reload());
        }
    }

    private IEnumerator Reload() {
        isReloading = true;
        UpdateReloadingText();

        yield return new WaitForSeconds(activeWeaponSO.reloadTime);
        weaponsCurrentAmmo[weapons[currentWeaponIndex]] = activeWeaponSO.magazineSize;
        UpdateAmmoCount();
        isReloading = false;
        UpdateReloadingText();
    }

    

    private void PlayActiveWeaponSound() {
        audioSource.PlayOneShot(activeClip);
    }

    private void UpdateAmmoCount() {
        ammoCount.text = weaponsCurrentAmmo[weapons[currentWeaponIndex]].ToString() + "/" + activeWeaponSO.magazineSize;
    }
    private void UpdateReloadingText() {
        reloadingText.gameObject.SetActive(isReloading);
    }

    public WeaponSO GetActiveWeaponSO() {
        return activeWeaponSO;
    }

    public Transform GetHitmarkerTransform() {
        return hitmarker;
    }



}
