using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathZone : MonoBehaviour {

    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<HealthSystem>() != null) {
            other.GetComponent<HealthSystem>().Damage(999);
        }
        
    }

}
