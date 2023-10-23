using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RangedWeapon : MonoBehaviour {

    [SerializeField] private Transform firePoint;
    [SerializeField] private WeaponSO weaponSO;
    [SerializeField] private GameInput gameInput;
    private Vector3 mouseWorldPosition;
    


    

    private Vector3 CalculateAimDirection() {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 aimDirection = (mousePosition - Camera.main.WorldToScreenPoint(firePoint.position)).normalized;
        return aimDirection;
    }
}
