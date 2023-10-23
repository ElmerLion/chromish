using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Bullet : MonoBehaviour {

    private float speed = 60f;

    private WeaponSO weaponSO;
    private Transform hitmarker;
    public bool isPlayer;
    private GameObject bulletImpact;

    public void SetWeapon(WeaponSO weapon) {
        weaponSO = weapon;
    }

    public void SetIsPlayer() {
        isPlayer = true;
    }

    private void Start() {
        speed = 80;
        hitmarker = WeaponManager.Instance.GetHitmarkerTransform();
        bulletImpact = Resources.Load<GameObject>("BulletImpact");
    }

    private void Update() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }
    private void HandleHit(Collider collider) {
        HealthSystem targetHealthSystem = collider.GetComponent<HealthSystem>();
        if (targetHealthSystem != null) {
            targetHealthSystem.Damage(weaponSO.damage);
            if (isPlayer) {
                SoundManager.Instance.PlayHitMarkerSound();
            }
        }

        if (bulletImpact != null) {
            Instantiate(bulletImpact, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other) {
        HandleHit(other);
    }
}
