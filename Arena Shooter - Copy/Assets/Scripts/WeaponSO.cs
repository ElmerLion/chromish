using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/WeaponSO")]
public class WeaponSO : ScriptableObject {

    public string nameString;
    public int damage;
    public int magazineSize;
    public int maxMagazines;
    public float fireRate;
    public float reloadTime;

}
